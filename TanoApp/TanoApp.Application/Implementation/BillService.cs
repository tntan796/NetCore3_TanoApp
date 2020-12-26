using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Data.Entities;
using TanoApp.Data.Enums;
using TanoApp.Data.IRepositories;
using TanoApp.Infrastructure.Interfaces;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Implementation
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IBillDetailRepository _billDetailRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private IMapper _mapper;
        public BillService(
            IBillRepository billRepository,
            IBillDetailRepository billDetailRepository,
            IColorRepository colorRepository,
            ISizeRepository sizeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IProductRepository productRepository)
        {
            _billRepository = billRepository;
            _billDetailRepository = billDetailRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public void Create(BillViewModel billVm)
        {
            var bill = _mapper.Map<BillViewModel, Bill>(billVm);
            var billDetails = _mapper.Map< List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            foreach(var detail in billDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
            }
            bill.BillDetails = billDetails;
            _billRepository.Add(bill);
        }

        public BillDetailViewModel CreateBillDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = _mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _billDetailRepository.Add(billDetail);
            return billDetailVm;
        }

        public void DeleteDetail(int productId, int billId, int colorId, int sizeId)
        {
            var detail = _billDetailRepository.FindSingle(x => x.ProductId == productId
                         && x.BillId == billId && x.ColorId == colorId && x.SizeId == sizeId);
            _billDetailRepository.Remove(detail);
        }

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyWord, int pageIndex, int pageSize)
        {
            var query = _billRepository.FindAll();
            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                query = query.Where(t => t.CustomerName.Contains(keyWord) || t.CustomerMobile.Contains(keyWord));
            }
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= start);
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated <= end);
            }
            var totalRow = query.Count();

            var t = query.OrderByDescending(x => x.DateCreated).ToList();
            var data = query.OrderByDescending(x => x.DateCreated)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            return new PagedResult<BillViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                RowCount = totalRow,
                Results = _mapper.Map<List<Bill>, List<BillViewModel>>(data)
            };
        }

        public List<BillDetailViewModel> GetBillDetails(int billId)
        {
            var products = _productRepository.FindAll();
            var colors = _colorRepository.FindAll();
            var sizes = _sizeRepository.FindAll();
            var billDetails = _billDetailRepository.FindAll(x => x.BillId == billId);
            var query =
                from b in billDetails
                join p in products on b.ProductId equals p.Id
                join s in sizes on b.SizeId equals s.Id
                join c in colors on b.ColorId equals c.Id
                where b.BillId == billId
                select new BillDetailViewModel() {
                    Bill = _mapper.Map<Bill, BillViewModel>(b.Bill),
                    BillId = billId,
                    ColorId = c.Id,
                    Color = _mapper.Map<Color, ColorViewModel>(c),
                    Id = b.Id,
                    Price = b.Price,
                    Product = _mapper.Map<Product, ProductViewModel>(p),
                    ProductId = p.Id,
                    Quantity = b.Quantity,
                    Size = _mapper.Map<Size, SizeViewModel>(s),
                    SizeId = s.Id
                };
            return query.ToList();
        }

        public List<ColorViewModel> GetColors()
        {
            var colors = _colorRepository.FindAll().ToList();
            return _mapper.Map<List<Color>, List<ColorViewModel>>(colors);
        }

        public BillViewModel GetDetail(int billId)
        {
            var bill = _billRepository.FindSingle(x => x.Id == billId);
            var billVm = _mapper.Map<Bill, BillViewModel>(bill);
            var billDetailVm = _billDetailRepository.FindAll(x => x.BillId == billId).ToList();
            billVm.BillDetails = _mapper.Map<List<BillDetail>, List<BillDetailViewModel>>(billDetailVm);
            return billVm;
        }

        public List<SizeViewModel> GetSizes()
        {
            var sizes = _sizeRepository.FindAll().ToList();
            return _mapper.Map<List<Size>, List<SizeViewModel>>(sizes);
        }

        public void Update(BillViewModel billVm)
        {
            var bill = _mapper.Map<BillViewModel, Bill>(billVm);
            var newDetails = bill.BillDetails;
            var addDetails = newDetails.Where(x => x.Id == 0).ToList();
            var updateDetails = newDetails.Where(x => x.Id != 0).ToList();
            var existDetails = _billDetailRepository.FindAll(x => x.BillId == billVm.Id).ToList();
            bill.BillDetails.Clear();
            foreach (var detail in updateDetails)
            {
                _billDetailRepository.Update(detail);
            }
            foreach (var detail in addDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                _billDetailRepository.Add(detail);
            }
            var except = existDetails.Except(updateDetails).ToList();

            _billDetailRepository.RemoveMultiple(except);
            _billRepository.Update(bill);
        }

        public void UpdateStatus(int orderId, BillStatus status)
        {
            var bill = _billRepository.FindById(orderId);
            bill.BillStatus = status;
            _billRepository.Update(bill);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            try
            {
                var bill = _billRepository.FindById(id);
                _billRepository.Remove(bill);
                Save();
            }
            catch (Exception ex)
            {
                throw;
            }
          
        }
    }
}
