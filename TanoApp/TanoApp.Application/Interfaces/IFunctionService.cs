using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TanoApp.Application.ViewModels.System;

namespace TanoApp.Application.Interfaces
{
    public interface IFunctionService: IDisposable
    {
        Task<List<FunctionViewModel>> GetAll(string filter);
        void Add(FunctionViewModel functionVm);
        List<FunctionViewModel> GetAllWithParentId(string parentId);
        FunctionViewModel GetById(string id);
        void Update(FunctionViewModel functionVm);
        void Delete(string id);
        void Save();
        bool CheckExistedId(string id);
        void UpdateParentId(string sourceId, string targetID, Dictionary<string, int> items);
        void ReOrder(string sourceId, string targetId);
    }
}
