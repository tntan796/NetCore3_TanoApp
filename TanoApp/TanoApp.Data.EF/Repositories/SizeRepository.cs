using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class SizeRepository: EFRepository<Size, int>, ISizeRepository
    {
        public SizeRepository(AppDbContext appContext): base(appContext)
        {
        }
    }
}
