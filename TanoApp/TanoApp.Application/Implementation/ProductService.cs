using AutoMapper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        IProductQuantityRepository _productQuantityRepository;
        IProductImageRepository _productImageRepository;
        IWholePriceRepository _wholePriceRepository;
        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            IProductTagRepository productTagRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IProductQuantityRepository productQuantityRepository,
            IProductImageRepository productImageRepository,
            IWholePriceRepository wholePriceRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productTagRepository = productTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _productQuantityRepository = productQuantityRepository;
            _productImageRepository = productImageRepository;
            _wholePriceRepository = wholePriceRepository;
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

        public void ImportExcel(string filePath, int categoryId)
        {
            using(var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                Product product;
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    product = new Product();
                    product.CategoryId = categoryId;
                    product.Name = workSheet.Cells[i, 1].Value.ToString();
                    product.Description = workSheet.Cells[i, 2].Value.ToString();
                    decimal.TryParse(workSheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    product.OriginalPrice = originalPrice;
                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString(), out var promotionPrice);
                    product.PromotionPrice = promotionPrice;
                    product.Content = workSheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = workSheet.Cells[i, 7].Value.ToString();
                    product.SeoDescription = workSheet.Cells[i, 8].Value.ToString();
                    bool.TryParse(workSheet.Cells[i, 9].Value.ToString(), out var hotFlag);
                    product.HotFlag = hotFlag;
                    bool.TryParse(workSheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;
                    product.Status = Status.Active;
                    _productRepository.Add(product);
                }
            }
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
            }
            var product = _mapper.Map<ProductViewModel, Product>(productVm);
            foreach (var tag in productTags)
            {
                product.ProductTags.Add(tag);
            }
            _productRepository.Update(product);
        }

        public List<ProductQuantityViewModel> GetQuantities(int productId)
        {
            var productQuantities = _productQuantityRepository.FindAll(x => x.ProductId == productId).ToList();
            return _mapper.Map<List<ProductQuantity>, List<ProductQuantityViewModel>>(productQuantities);
        }

        public void AddQuantity(int productId, List<ProductQuantityViewModel> quantities)
        {
            var oldQuantity = _productQuantityRepository.FindAll(x => x.ProductId == productId).ToList();
            _productQuantityRepository.RemoveMultiple(oldQuantity);
            foreach( var quantity in quantities)
            {
                var newProductQuantity = new ProductQuantity();
                newProductQuantity.ProductId = productId;
                newProductQuantity.ColorId = quantity.ColorId;
                newProductQuantity.SizeId = quantity.SizeId;
                newProductQuantity.Quantity = quantity.Quantity;
                _productQuantityRepository.Add(newProductQuantity);
            }
        }

        public void AddImages(int productId, string[] images)
        {
            _productImageRepository.RemoveMultiple(_productImageRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach(var image in images)
            {
                _productImageRepository.Add(new ProductImage()
                {
                    Path = image,
                    ProductId = productId,
                    Caption = string.Empty
                });
            }
        }
        public List<ProductImageViewModel> GetImages(int productId)
        {
            var productImages = _productImageRepository.FindAll(x => x.ProductId == productId).ToList();
            return _mapper.Map<List<ProductImage>, List<ProductImageViewModel>>(productImages);
        }

        public void AddWholePrices(int productId, List<WholePriceViewModel> wholePrices)
        {
            var oldWholePrices = _wholePriceRepository.FindAll(x => x.ProductId == productId).ToList(); ;
            _wholePriceRepository.RemoveMultiple(oldWholePrices);
            foreach(var wholePrice in wholePrices)
            {
                _wholePriceRepository.Add(new WholePrice()
                {
                    ProductId = productId,
                    FromQuantity = wholePrice.FromQuantity,
                    ToQuantity = wholePrice.ToQuantity,
                    Price = wholePrice.Price
                });

            }
        }

        public List<WholePriceViewModel> GetWholePrice(int productId)
        {
            var wholePrices = _wholePriceRepository.FindAll(x => x.ProductId == productId).ToList();
            return _mapper.Map<List<WholePrice>, List<WholePriceViewModel>>(wholePrices);
        }
    }
}
