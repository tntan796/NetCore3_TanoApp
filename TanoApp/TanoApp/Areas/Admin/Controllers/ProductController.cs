using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Products;

namespace TanoApp.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ProductViewModel> productViewModels = _productService.GetListProduct();
            return Ok(productViewModels);
        }
    }
}
