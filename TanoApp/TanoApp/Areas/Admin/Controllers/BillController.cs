using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Common;
using TanoApp.Application.ViewModels.Products;
using TanoApp.Data.Enums;
using TanoApp.Utilities.Extensions;
using TanoApp.Utilities.Helpers;

namespace TanoApp.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        private readonly IBillService _billService;
        private readonly IHostingEnvironment _hostingEnviroment;

        public BillController(IBillService billService,
                              IHostingEnvironment hostingEnvironment)
        {
            _billService = billService;
            _hostingEnviroment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var bill = _billService.GetDetail(id);
            return Ok(bill);
        }

        [HttpPut]
        public IActionResult UpdateStatus(int billId, BillStatus status)
        {
            _billService.UpdateStatus(billId, status);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllPaging(string startDate, string endDate, string keyWord, int pageIndex, int pageSize)
        {
            var model = _billService.GetAllPaging(startDate, endDate, keyWord, pageIndex, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(BillViewModel billVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(x => x.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (billVm.Id == 0)
            {
                _billService.Create(billVm);
            } else
            {
                _billService.Update(billVm);
            }
            _billService.Save();
            return new OkObjectResult(billVm);
        }

        [HttpGet]
        public IActionResult GetPaymentMethod()
        {
            List<EnumModel> enums = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                                    .Select(c => new EnumModel()
                                    {
                                        Value = (int)c,
                                        Name = c.GetDescription()
                                    }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpGet]
        public IActionResult GetBillStatus()
        {
            List<EnumModel> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
                                    .Select(c => new EnumModel()
                                    {
                                        Value = (int)c,
                                        Name = c.GetDescription()
                                    }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpPost]
        public IActionResult ExportExcel(int billId)
        {
            string sWebRootFolder = _hostingEnviroment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string sFileName = $"Bill_{billId}.xlsx";
            string templateDocument = Path.Combine(sWebRootFolder, "templates", "BillTemplate.xlsx");
            string url = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, "export-files", sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using(FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using(ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["TanoOrder"];
                    // Data access, load order header data
                    var billDetail = _billService.GetDetail(billId);
                    //Insert customer data into template
                    worksheet.Cells[4, 1].Value = "Name: " + billDetail.CustomerName;
                    worksheet.Cells[5, 1].Value = "Address: " + billDetail.CustomerAddress;
                    worksheet.Cells[6, 1].Value = "Phone: " + billDetail.CustomerMobile;
                    // Start row for detail row
                    int rowIndex = 9;
                    // Load order details
                    var orderDetails = _billService.GetBillDetails(billId);
                    int count = 1;
                    foreach(var orderDetail in orderDetails)
                    {
                        // Cell 1, 
                        worksheet.Cells[rowIndex, 1].Value = count.ToString();
                        // Cell2, Order number 
                        worksheet.Cells[rowIndex, 2].Value = orderDetail.Product.Name;
                        // C
                        worksheet.Cells[rowIndex, 3].Value = orderDetail.Quantity.ToString();
                        worksheet.Cells[rowIndex, 4].Value = orderDetail.Price.ToString();
                        worksheet.Cells[rowIndex, 5].Value = (orderDetail.Price * orderDetail.Quantity).ToString("N0");
                        rowIndex++;
                        count++;
                    }
                    decimal total = (decimal)(orderDetails.Sum(x => x.Quantity * x.Price));
                    worksheet.Cells[24, 5].Value = total.ToString("N0");
                    var numberWord = "Total amount (by words): " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;
                    var billDate = billDetail.DateCreated;
                    worksheet.Cells[28, 3].Value = "Day " + billDate.Day + " month " + billDate.Month + " year" + billDate.Year;
                    package.SaveAs(file); // Savethe workbook
                }
            }
            return new OkObjectResult(url);
        }
   
        [HttpGet]
        public IActionResult GetColors()
        {
            var colors = _billService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public IActionResult GetSizes()
        {
            var sizes = _billService.GetSizes();
            return new OkObjectResult(sizes);
        }

        [HttpDelete] 
        public IActionResult Delete(int id)
        {
            _billService.Delete(id);
            return Ok(id);
        }
    }
}