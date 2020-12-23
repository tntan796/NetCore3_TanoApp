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
        private IMapper _mapper;
        public BillService(
            IBillRepository billRepository,
            IBillDetailRepository billDetailRepository,
            IColorRepository colorRepository,
            ISizeRepository sizeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _billRepository = billRepository;
            _billDetailRepository = billDetailRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Create(BillViewModel billVm)
        {
            var bill = _mapper.Map<BillViewModel, Bill>(billVm);
            var billDetails = _mapper.Map< List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
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
            var billDetails =  _billDetailRepository
                                .FindAll(x => x.BillId == billId,
                                         c => c.Bill,
                                         c => c.Color,
                                         c => c.Size,
                                         c => c.Product)
                                .ToList();
            return _mapper.Map<List<BillDetail>, List<BillDetailViewModel>>(billDetails);
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
            var existDetails = _billDetailRepository.FindAll(x => x.BillId == billVm.Id);
            bill.BillDetails.Clear();
            foreach (var detail in updateDetails)
            {
                _billDetailRepository.Update(detail);
            }
            foreach (var detail in addDetails)
            {
                _billDetailRepository.Add(detail);
            }
            _billDetailRepository.RemoveMultiple(existDetails.Except(updateDetails).ToList());
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
    }
}
