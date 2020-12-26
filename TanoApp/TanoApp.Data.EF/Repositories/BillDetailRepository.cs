using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public  class BillDetailRepository: EFRepository<BillDetail, int>, IBillDetailRepository
    {
        public BillDetailRepository(AppDbContext appDbContext): base(appDbContext)
        {
        }
    }
}
