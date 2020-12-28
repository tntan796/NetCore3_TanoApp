using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.Entities;
using TanoApp.Infrastructure.Interfaces;

namespace TanoApp.Data.IRepositories
{
    public interface IProductImageRepository : IRepository<ProductImage, int>
    {
    }
}
