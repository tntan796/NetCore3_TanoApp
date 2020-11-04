using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TanoApp.Application.ViewModels.Products;

namespace TanoApp.Application.Interfaces
{
    public interface IProductService: IDisposable
    {
        Task<List<ProductViewModel>> GetListProduct();
    }
}
