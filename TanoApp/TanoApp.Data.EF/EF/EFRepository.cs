using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanoApp.Infrastructure.Interfaces;
using TanoApp.Infrastructure.ShareKernel;

namespace TanoApp.Data.EF.EF
{
    public class EFRepository<T, K> : IRepository<T, K>, IDisposable where T: DomainEntity<K>
    {
        private AppDbContext _dbContext;
        public EFRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
        {
            _dbContext.Add(entity);
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        public System.Linq.IQueryable<T> FindAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _dbContext.Set<T>(); // Chọn đến kiểu T
            if (includeProperties != null )
            {
                foreach(var includeProperty in includeProperties)
                {
                    items.Include(includeProperty);
                }
            }
            return items;
        }

        public System.Linq.IQueryable<T> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _dbContext.Set<T>(); // Chọn đến kiểu T
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public T FindById(K id, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(x => x.Id.Equals(id));
        }

        public T FindSingle(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            return FindAll(includeProperties).SingleOrDefault(predicate);
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Remove(K id)
        {
            Remove(FindById(id));
        }

        public void RemoveMultiple(List<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
