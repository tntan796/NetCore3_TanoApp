using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TanoApp.Views.Home
{
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Error 404
        /// </summary>
        /// <returns></returns>
        public IActionResult PageNotFound()
        {
            return View();
        }

        /// <summary>
        /// Error 500
        /// </summary>
        /// <returns></returns>
        public IActionResult InternalServerError()
        {
            return View();
        }
        /// <summary>
        /// Error 503
        /// </summary>
        /// <returns></returns>
        public IActionResult ServiceUnavailable()
        {
            return View();
        }
        
        /// <summary>
        /// Error 400
        /// </summary>
        /// <returns></returns>
        public IActionResult BadRequest()
        {
            return View();
        }
    }
}
