using Dgm.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dgm.Core.SeedWork
{
    public abstract class Repository<T, TdbContext> : IRepository<T> where T : class, IAggregateRoot where TdbContext : BaseDbContext
    {
        public IUnitOfWork Uow => _dbContext;

        protected TdbContext _dbContext;

        protected Repository(TdbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }

      
        public async Task CommitAsync()
        {
            await Uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().Where(t => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int entityId)
        {
            return await _dbContext.Set<T>().FindAsync(entityId);
        }

        public T Add(T entity)
        {
            var T = _dbContext.Set<T>().Add(entity).Entity;
            return T;
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(int entityId)
        {
            var entity = _dbContext.Find<T>(entityId);
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

    }
}
