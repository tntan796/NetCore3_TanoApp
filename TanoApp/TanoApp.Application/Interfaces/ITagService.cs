using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Application.ViewModels.Common;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Interfaces
{
    public interface ITagService
    {
        List<TagViewModel> GetListTag();
        PagedResult<TagViewModel> GetAllPaging(string keyword, int page, int pageSize);
        TagViewModel Add(TagViewModel tag);
        void Update(TagViewModel tag);
        void Delete(string id);
        TagViewModel GetById(string id);
        void Save();
    }
}
