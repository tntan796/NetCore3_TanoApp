using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Application.ViewModels.Common;
using TanoApp.Application.ViewModels.Products;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Interfaces
{
    public interface IProductTagService
    {
        List<ProductTagViewModel> GetListTag();
        PagedResult<ProductTagViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
        ProductTagViewModel Add(ProductTagViewModel product);
        void Update(ProductTagViewModel product);
        void Delete(int id);
        ProductTagViewModel GetById(int id);
        void Save();
    }
}
