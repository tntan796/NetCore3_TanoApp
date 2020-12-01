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
            _productCategoryRepository.Add(product);
            return productCategoryVm;
        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            List<ProductCategory> productCategories = _productCategoryRepository.FindAll().OrderBy(x => x.ParentId).Select(x => x).ToList();
            List<ProductCategoryViewModel> productCategoryViewModels = _mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(productCategories);
            return productCategoryViewModels;
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            List<ProductCategory> productCategories = new List<ProductCategory>();
            if (!string.IsNullOrEmpty(keyword))
                productCategories = _productCategoryRepository.FindAll(x => x.Name.Contains(keyword) || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).ToList();
            else
                productCategories = _productCategoryRepository.FindAll().OrderBy(x => x.ParentId).ToList();
            return _mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(productCategories);
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            List<ProductCategory> productCategories = _productCategoryRepository.FindAll(x => x.Status == Status.Active
            && x.ParentId == parentId).ToList();
            return _mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(productCategories);
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return _mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            List<ProductCategory> productCategories = _productCategoryRepository
            .FindAll(x => x.HomeFlag == true, c => c.Products)
              .OrderBy(x => x.HomeOrder)
              .Take(top).ToList();
            //foreach (var category in categories)
            //{
                //category.Products = _productRepository
                //    .FindAll(x => x.HotFlag == true && x.CategoryId == category.Id)
                //    .OrderByDescending(x => x.DateCreated)
                //    .Take(5)
                //    .ProjectTo<ProductViewModel>().ToList();
            //}
            return _mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(productCategories);
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
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
            var sibling = _productCategoryRepository.FindAll().ToList().Where(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }
    }
}
