using System.Collections.Generic;
using System.Linq;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class ProductCategoryRepository : EFRepository<ProductCategory, int>, IProductCategoryRepository
    {
        private AppDbContext _context;
        public ProductCategoryRepository(AppDbContext context): base(context) {
            _context = context;
        }

        public List<ProductCategory> GetByAlias(string alias)
        {
            return _context.ProductCategories.Where(t => t.SeoAlias == alias).ToList();
        }
        // Các phương thức cơ bản như Add, Update, Delete đã được thêm trong EFRepository rồi nếu không làm việc gì khác thì không cần viết lại

    }
}
