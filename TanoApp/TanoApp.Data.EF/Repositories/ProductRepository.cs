using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class ProductRepository: EFRepository<Product, int>, IProductRepository
    {
        AppDbContext _dbContext;
        public ProductRepository(AppDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
