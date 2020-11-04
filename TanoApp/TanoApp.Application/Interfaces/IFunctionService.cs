using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TanoApp.Application.ViewModels.System;

namespace TanoApp.Application.Interfaces
{
    public interface IFunctionService: IDisposable
    {
        Task<List<FunctionViewModel>> GetAll();
        Task<List<FunctionViewModel>> GetAllByPermission(Guid userId);
    }
}
