using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.System;
using TanoApp.Authorization;

namespace TanoApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        public UserController(IUserService userService,
                              IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (result.Succeeded == false)
            {
                return new RedirectResult("/Admin/Login/Index");
            }
            return View();
        }

        public IActionResult GetAll()
        {
            var model = _userService.GetAllAsync();
            return Ok(model);
        }

        public async Task<IActionResult> GetById(string id)
        {
            var model = await _userService.GetById(id);
            return Ok(model);
        }

        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _userService.GetAllPagingAsync(keyword, page, pageSize);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppUserViewModel userVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Ok(allErrors);
            }
            else
            {
                if (userVm.Id == null)
                {
                    await _userService.AddAsync(userVm);
                } else
                {
                    await _userService.UpdateAsync(userVm);
                }
                return Ok(userVm);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState.Values.SelectMany(x => x.Errors));
            } else
            {
                await _userService.DeleteAsync(id);
                return Ok(id);
            }
        }
    }
}