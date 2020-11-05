using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TanoApp.Extensions;

namespace TanoApp.Areas.Admin.Components
{
    public class MenuViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = ((ClaimsPrincipal)User);
            var data = new {
                UserName = user.GetSpecificClaim("UserName"),
                Email = user.GetSpecificClaim("Email"),
                FullName = user.GetSpecificClaim("FullName")
            };
            ViewBag.data = data;
            return View(data);
        }
    }
}
