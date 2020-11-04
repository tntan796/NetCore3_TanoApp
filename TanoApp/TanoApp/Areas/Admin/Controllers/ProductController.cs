using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Products;

namespace TanoApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            List<ProductViewModel> productViewModels = await _productService.GetListProduct();
            return View(productViewModels);
        }
    }
}
