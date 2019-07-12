using EverCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EverCoach.Repository
{
    public abstract class DataRepository<T>:IDataRepository<T> where T: class
    {
        protected CoachContext _coachContext { get; set; }
        public DataRepository(CoachContext coachContext)
        {
            _coachContext = coachContext;
        }

        public IQueryable<T> FindAll()
        {
            return _coachContext.Set<T>();
        }
        public IQueryable<T>  FindByCondition(Expression<Func<T,bool>> expression)
        {
            return _coachContext.Set<T>().Where(expression);
        }
        public void Create(T entity)
        {
            _coachContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _coachContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _coachContext.Set<T>().Remove(entity);
        }
        public async Task SaveAsync()
        {
            await _coachContext.SaveChangesAsync();
        }
    }
}
