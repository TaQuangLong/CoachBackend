using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dgm.Core.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot 
    {
        IUnitOfWork Uow { get; }
        Task CommitAsync();

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int entityId);

        T Add(T entity);
        void Delete(T entity);
        void Delete(int entityId);
        void Update(T entity);
    }
}
