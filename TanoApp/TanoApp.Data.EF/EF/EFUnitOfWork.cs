using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Infrastructure.Interfaces;

namespace TanoApp.Data.EF.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private AppDbContext _dbContext;
        public EFUnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
