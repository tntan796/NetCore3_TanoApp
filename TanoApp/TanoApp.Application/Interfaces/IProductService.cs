using System;
using System.Collections.Generic;
using TanoApp.Application.ViewModels.Products;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Interfaces
{
    public interface IProductService: IDisposable
    {
        List<ProductViewModel> GetListProduct();
        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
        ProductViewModel Add(ProductViewModel product);
        void Update(ProductViewModel product);
        void Delete(int id);
        ProductViewModel GetById(int id);
        void Save();
        void ImportExcel(string filePath, int categoryId);
        void AddQuantity(int productId, List<ProductQuantityViewModel> quantities);
        List<ProductQuantityViewModel> GetQuantities(int productId);

        void AddImages(int productId, string[] images);
        List<ProductImageViewModel> GetImages(int productId);

        void AddWholePrices(int productId, List<WholePriceViewModel> wholePrices);
        List<WholePriceViewModel> GetWholePrice(int productId);
    }
}
