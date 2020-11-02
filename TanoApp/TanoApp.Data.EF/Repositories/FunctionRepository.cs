using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class FunctionRepository: EFRepository<Function, string>, IFunctionRepository
    {
        public FunctionRepository(AppDbContext dbContext): base(dbContext)
        {
        }
    }
}
