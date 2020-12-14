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
using TanoApp.Infrastructure.Interfaces;
using TanoApp.Utilities.Constants;
using TanoApp.Utilities.Helpers;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Implementation
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        IMapper _mapper;
        IProductTagRepository _productTagRepository;
        ITagRepository _tagRepository;
        IUnitOfWork _unitOfWork;
        public ProductService(IProductRepository productRepository, IMapper mapper, IProductTagRepository productTagRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productTagRepository = productTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductViewModel Add(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach(string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
                var product = _mapper.Map<ProductViewModel, Product>(productVm);
                foreach (var tag in productTags) {
                    product.ProductTags.Add(tag);
                }
                _productRepository.Add(product);

            }
            return productVm;
        }

        public void Delete(int id)
        {
            _productRepository.Remove(id);
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
            return _mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
        }

        public List<ProductViewModel> GetListProduct()
        {
            List<Product> products = _productRepository.FindAll(x => x.ProductCategory).ToList();
            return _mapper.Map<List<Product>, List<ProductViewModel>>(products);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductViewModel productVm)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(productVm.Tags))
            {
                string[] tags = productVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    _productTagRepository.RemoveMultiple(
                                            _productTagRepository
                                            .FindAll(x => x.Id == productVm.Id)
                                         .ToList());
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }
                var product = _mapper.Map<ProductViewModel, Product>(productVm);
                foreach (var tag in productTags)
                {
                    product.ProductTags.Add(tag);
                }
                _productRepository.Update(product);
            }
        }
    }
}
