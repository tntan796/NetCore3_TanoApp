using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.System;
using TanoApp.Extensions;

namespace TanoApp.Areas.Admin.Components
{
    public class SideBarViewComponent: ViewComponent
    {
        IFunctionService _functionService;
        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functionViewModels;
            if (roles.Split(";").Contains("Admin"))
            {
                functionViewModels = await _functionService.GetAll(string.Empty);
            } else
            {
                functionViewModels = new List<FunctionViewModel>();
            }
            return View(functionViewModels);
        }
    }
}
