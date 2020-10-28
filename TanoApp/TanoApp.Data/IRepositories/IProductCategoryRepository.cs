using System.Collections.Generic;
using TanoApp.Data.Entities;
using TanoApp.Infrastructure.Interfaces;

namespace TanoApp.Data.IRepositories
{
    interface IProductCategoryRepository: IRepository<ProductCategory, int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}
