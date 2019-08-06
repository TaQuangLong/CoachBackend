using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverCoach.Domain.AggregatesModel.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot 
    {
        IUnitOfWork UnitOfWork { get; }

        
    }
}
