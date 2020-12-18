using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.System;

namespace TanoApp.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IFunctionService _functionService;
        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var model = await _functionService.GetAll(string.Empty);
            var root = model.Where(c => c.ParentId == null);
            var items = new List<FunctionViewModel>();
            foreach(var function in root)
            {
                // Add the parent category to the item list
                items.Add(function);
                GetByParentId(model.ToList(), function, items);
            }
            return Ok(items);
        }

        private void GetByParentId(IEnumerable<FunctionViewModel> allFunctions,
            FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var functionsEntities = allFunctions as FunctionViewModel[] ?? allFunctions.ToArray();
            var subFunctions = functionsEntities.Where(x => x.ParentId == parent.Id);
            foreach(var cat in subFunctions)
            {
                // Add this category
                items.Add(cat);
                // Recursive call in case your have a hierachy more than 1 level deep
                GetByParentId(functionsEntities, cat, items);
            }
        }
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            } else
            {
                _functionService.Delete(id);
                _functionService.Save();
                return new OkObjectResult(id);
            }
        }

        [HttpPost]
        public IActionResult ReOrder(string sourceId, string targetId)
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
                    _functionService.ReOrder(sourceId, targetId);
                    _functionService.Save();
                    return new OkObjectResult(sourceId);
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
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
                    _functionService.UpdateParentId(sourceId, targetId, items);
                    _functionService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public IActionResult SaveEntity(FunctionViewModel functionVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(x => x.Errors);
                return new BadRequestObjectResult(allErrors);
            } else
            {
                if (string.IsNullOrWhiteSpace(functionVm.Id))
                {
                    _functionService.Add(functionVm);
                } else
                {
                    _functionService.Update(functionVm);
                }
                _functionService.Save();
                return new OkObjectResult(functionVm);
            }
        }

        [HttpGet]
        public IActionResult GetAllFilter(string search)
        {
            var model = _functionService.GetAll(search);
            return Ok(model);
        }
    }
}