using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Utilities.Helpers;

namespace TanoApp.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        IProductService _productService;
        IProductCategoryService _productCategoryService;
        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
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

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allError = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allError);
            } else
            {
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.Id == 0)
                {
                    _productService.Add(productVm);
                } else
                {
                    _productService.Update(productVm);
                }
                _productService.Save();
                return new OkObjectResult(productVm);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            }
            else
            {
                _productService.Delete(id);
                _productService.Save();
                return new OkObjectResult(id);
            }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            var model = _productService.GetById(id);
            return Ok(model);
        }
    }
}
