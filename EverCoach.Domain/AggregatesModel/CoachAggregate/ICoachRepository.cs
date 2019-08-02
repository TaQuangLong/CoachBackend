using EverCoach.Domain.AggregatesModel.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverCoach.Domain.AggregatesModel.CoachAggregate
{
    public interface ICoachRepository:IRepository<Coach>
    {
        Task CommitAsync();

        Task<IList<Coach>> GetAllAsync();

        Task<Coach> GetByIdAsync(Guid entityId);

        Coach Add(Coach entity);
        void Update(Coach entity);
        void Delete(Coach entity);
        void Delete(Guid entityId);
    }
}
