using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Data.Entities;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Interfaces
{
    public interface IProductService: IDisposable
    {
        List<ProductViewModel> GetListProduct();
        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);
    }
}
