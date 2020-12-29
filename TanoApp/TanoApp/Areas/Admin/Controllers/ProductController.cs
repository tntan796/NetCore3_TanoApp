using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Data.Entities;
using TanoApp.Utilities.Helpers;

namespace TanoApp.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProductController(IProductService productService,
                                 IProductCategoryService productCategoryService,
                                 IHostingEnvironment hostingEnvironment)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var fileName = file.FileName;
                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, fileName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                _productService.ImportExcel(filePath, categoryId);
                _productService.Save();
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult ExportExcel()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Product_{DateTime.Now: yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder + "\\export-files", sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var products = _productService.GetListProduct();
            using(ExcelPackage package = new ExcelPackage(file))
            {
                // Add a new worksheet to the empty
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells["A1"].LoadFromCollection(products, true, OfficeOpenXml.Table.TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); // Save the workbook
            }
            return new OkObjectResult(fileUrl);
        }
    
        [HttpPost]
        public IActionResult SaveQuantities(int productId, List<ProductQuantityViewModel> quantities)
        {
            _productService.AddQuantity(productId, quantities);
            _productService.Save();
            return new OkObjectResult(quantities);
        }

        [HttpGet]
        public IActionResult GetQuantities(int productId)
        {
            var quantities = _productService.GetQuantities(productId);
            return new OkObjectResult(quantities);
        }

        [HttpGet]
        public IActionResult GetImages(int productId)
        {
            return Ok(_productService.GetImages(productId));
        }

        [HttpPost]
        public IActionResult AddImages(int productId, string[] images)
        {
            _productService.AddImages(productId, images);
            _productService.Save();
            return new OkObjectResult(images);
        }

        [HttpGet]
        public IActionResult GetWholePrice(int productId)
        {
            return Ok(_productService.GetWholePrice(productId));
        }

        [HttpPost]
        public IActionResult AddWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _productService.AddWholePrices(productId, wholePrices);
            _productService.Save();
            return Ok(wholePrices);
        }
    }
}
