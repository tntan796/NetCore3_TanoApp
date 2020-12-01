using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Data.Entities;
using TanoApp.Data.Enums;
using TanoApp.Data.IRepositories;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public ProductViewModel Add(ProductViewModel product)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            int totalRow = query.Count();
            query = query.OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
            var products = query.ToList();
            var data = _mapper.Map<List<Product>, List<ProductViewModel>>(products);
            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public ProductViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ProductViewModel> GetListProduct()
        {
            List<Product> products = _productRepository.FindAll(x => x.ProductCategory).ToList();
            return _mapper.Map<List<Product>, List<ProductViewModel>>(products);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(ProductViewModel product)
        {
            throw new NotImplementedException();
        }
    }
}
