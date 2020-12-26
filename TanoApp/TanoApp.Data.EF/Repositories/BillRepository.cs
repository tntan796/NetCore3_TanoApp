using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class BillRepository: EFRepository<Bill, int>, IBillRepository
    {
        public BillRepository(AppDbContext appDbContext): base(appDbContext)
        {
        }
    }
}
