using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Data.Entities;
using TanoApp.Data.Enums;
using TanoApp.Data.IRepositories;
using TanoApp.Infrastructure.Interfaces;

namespace TanoApp.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        IProductCategoryRepository _productCategoryRepository;
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository,
                                      IUnitOfWork unitOfWork,
                                      IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var product = _mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            var product1 = _mapper.Map<ProductCategory, ProductCategoryViewModel>(product);
            _productCategoryRepository.Add(product);
            return productCategoryVm;

        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            var mapper = _productCategoryRepository.FindAll().OrderBy(x => x.ParentId).Select(x => x).ToList();
            return _mapper.Map<List<ProductCategoryViewModel>>(mapper);
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            throw new NotImplementedException();
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            throw new NotImplementedException();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return _mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            throw new NotImplementedException();
        }

        public void ReOrder(int sourceId, int targetId)
        {
            //var sourceCategory = _productCategoryRepository.FindById(sourceId);
            //sourceCategory.ParentId = targetId;
            //_productCategoryRepository.Update(sourceCategory);

            ////Get all sibling
            //var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            //foreach (var child in sibling)
            //{
            //    child.SortOrder = items[child.Id];
            //    _productCategoryRepository.Update(child);
            //}
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = _mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Update(productCategory);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            _productCategoryRepository.Update(sourceCategory);

            //Get all sibling
            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }
    }
}
