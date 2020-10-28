using TanoApp.Data.Entities;
using TanoApp.Infrastructure.Interfaces;

namespace TanoApp.Data.IRepositories
{
    public interface IProductRepository: IRepository<Product, int>
    {
    }
}
