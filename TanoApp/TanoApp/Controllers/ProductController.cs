using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TanoApp.Controllers
{
    public class ProductController : Controller
    {
        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
