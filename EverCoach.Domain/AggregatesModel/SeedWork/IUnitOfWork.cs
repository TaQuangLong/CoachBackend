using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EverCoach.Domain.AggregatesModel.SeedWork
{
    public interface IUnitOfWork:IDisposable
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
