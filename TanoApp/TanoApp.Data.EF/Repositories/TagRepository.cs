using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class TagRepository: EFRepository<Tag, string>, ITagRepository
    {
        private AppDbContext _context;
        public TagRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
