using System;

namespace TanoApp.Infrastructure.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        /// <summary>
        /// Call SaveChanges function from dbContext
        /// </summary>
        void Commit();
    }
}
