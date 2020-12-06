using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Application.Interfaces;
using TanoApp.Application.ViewModels.Common;
using TeduCoreApp.Utilities.Dtos;

namespace TanoApp.Application.Implementation
{
    public class ProductTagService : ITagService
    {
        public TagViewModel Add(TagViewModel tag)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<TagViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public TagViewModel GetById(string id)
        {
            throw new NotImplementedException();
        }

        public List<TagViewModel> GetListTag()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(TagViewModel tag)
        {
            throw new NotImplementedException();
        }
    }
}
