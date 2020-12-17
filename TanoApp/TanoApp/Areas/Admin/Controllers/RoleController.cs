using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.System;

namespace TanoApp.Areas.Admin.Controllers
{
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetById(id);
            return Ok(role);
        }

        public IActionResult GetAllPaging(string keyword, int page, int pagesize)
        {
            return Ok(_roleService.GetAllPagingAsync(keyword, page, pagesize));
        }
        [HttpPost]
        public IActionResult SaveEntity(AppRoleViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(allErrors);
            } else
            {
                if (roleVm.Id == null)
                {
                    _roleService.AddAsync(roleVm);
                } else
                {
                    _roleService.UpdateAsync(roleVm);
                }
                return Ok(roleVm);
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            }
            else
            {
                await _roleService.DeleteAsync(id);
                return Ok(id);
            }
        }
    }
}