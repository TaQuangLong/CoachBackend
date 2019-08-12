using System.Threading;
using System.Threading.Tasks;

namespace SimpleTaskQueue.CronJobs.Scheduler
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
