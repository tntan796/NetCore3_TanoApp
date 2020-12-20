using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.System;
using TanoApp.Data.Entities;
using TanoApp.Data.Enums;
using TanoApp.Data.IRepositories;
using TanoApp.Infrastructure.Interfaces;

namespace TanoApp.Application.Implementation
{
    public class FunctionService: IFunctionService
    {
        IFunctionRepository _functionRepository;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        public FunctionService(IFunctionRepository functionRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public void Add(FunctionViewModel functionVm)
        {
            var function = _mapper.Map<FunctionViewModel, Function>(functionVm);
            _functionRepository.Add(function);
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<FunctionViewModel>> GetAll(string search)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (String.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.Name.Contains(search) || t.Id.Contains(search));
            }
            var functions = await query.ToListAsync();
            return _mapper.Map<List<Function>, List<FunctionViewModel>>(functions);
        }

        public List<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            var functions = _functionRepository.FindAll(x => x.ParentId == parentId).ToList();
            return _mapper.Map<List<Function>, List<FunctionViewModel>>(functions);
        }

        public FunctionViewModel GetById(string id)
        {
            var function = _functionRepository.FindById(id);
            return _mapper.Map<Function, FunctionViewModel>(function);
        }

        public void ReOrder(string sourceId, string targetId)
        {
            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;
            _functionRepository.Update(source);
            _functionRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel functionVm)
        {
            var functionDb = _functionRepository.FindById(functionVm.Id);
        }

        public void UpdateParentId(string sourceId, string targetID, Dictionary<string, int> items)
        {
            // Update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetID;
            _functionRepository.Update(category);

            // Get All sibling
            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach(var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _functionRepository.Update(child);
            }
        }
    }
}
