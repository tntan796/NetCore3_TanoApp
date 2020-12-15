using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TanoApp.Application.ViewModels.System;
using TanoApp.Data.Entities;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddAsync(AppRoleViewModel roleVm);
        Task DeleteAsync(Guid id);
        Task<List<AppRoleViewModel>> GetAllAsync();
        PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);
        Task<AppRoleViewModel> GetById(Guid id);
        Task UpdateAsync(AppRoleViewModel roleVm);
        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);
        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);
        Task<bool> checkpermission(string functionid, string action, string[] roles);
    }
}
