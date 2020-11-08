using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Utilities.Helpers;

namespace TanoApp.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        IProductCategoryService _productCategoryService;
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Get data API
        public IActionResult GetAll()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }
        [HttpPost]
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            } else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                } else
                {
                    _productCategoryService.UpdateParentId(sourceId, targetId, items);
                    _productCategoryService.Save();
                    return Ok();
                }
            }
        }
        [HttpPost]
        public IActionResult ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            } else
            {
                if(sourceId == targetId)
                {
                    return new BadRequestResult();
                } else
                {
                    _productCategoryService.ReOrder(sourceId, targetId);
                    _productCategoryService.Save();
                    return Ok();
                }
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var productCategory = _productCategoryService.GetById(id);
            return Ok(productCategory);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryViewModel productCategory)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(x => x.Errors);
                return new BadRequestObjectResult(modelErrors);
            } else
            {
                productCategory.SeoAlias = TextHelper.ToUnsignString(productCategory.Name);
                if (productCategory.Id == 0)
                {
                    _productCategoryService.Add(productCategory);
                } else
                {
                    _productCategoryService.Update(productCategory);
                }
                _productCategoryService.Save();
                return new ObjectResult(productCategory);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            } else
            {
                _productCategoryService.Delete(id);
                _productCategoryService.Save();
                return new OkResult();
            }
        }
        #endregion
    }
}