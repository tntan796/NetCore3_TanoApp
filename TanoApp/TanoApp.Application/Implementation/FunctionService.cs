using AutoMapper;
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
using TanoApp.Data.IRepositories;

namespace TanoApp.Application.Implementation
{
    public class FunctionService: IFunctionService
    {
        IFunctionRepository _functionRepostory;
        IMapper _mapper;
        public FunctionService(IFunctionRepository functionRepository, IMapper mapper)
        {
            _functionRepostory = functionRepository;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<FunctionViewModel>> GetAll()
        {
            var functions = await _functionRepostory.FindAll().ToListAsync();
            return _mapper.Map<List<Function>, List<FunctionViewModel>>(functions);
        }
        public Task<List<FunctionViewModel>> GetAllByPermission(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
