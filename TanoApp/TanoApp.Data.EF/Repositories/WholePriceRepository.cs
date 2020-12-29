using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class WholePriceRepository: EFRepository<WholePrice, int>, IWholePriceRepository
    {
        private readonly AppDbContext _context;
        public WholePriceRepository(AppDbContext context): base(context)
        {
            _context = context;
        }
    }
}
