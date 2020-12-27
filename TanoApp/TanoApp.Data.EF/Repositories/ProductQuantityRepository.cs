using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class ProductQuantityRepository: EFRepository<ProductQuantity, int>, IProductQuantityRepository
    {
        private AppDbContext _context;
        public ProductQuantityRepository(AppDbContext dbContext): base(dbContext)
        {
            _context = dbContext;
        }
    }
}
