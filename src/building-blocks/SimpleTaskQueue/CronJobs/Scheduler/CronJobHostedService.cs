using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleTaskQueue.CronJobs.Cron;

namespace SimpleTaskQueue.CronJobs.Scheduler
{
    public class SchedulerHostedService : BackgroundService
    {
        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;
        private readonly List<SchedulerTaskWrapper> _scheduledTasks = new List<SchedulerTaskWrapper>();
        private readonly ILogger<SchedulerHostedService> _logger;

        public SchedulerHostedService(
            IServiceProvider serviceProvider,
            ILogger<SchedulerHostedService> logger)
        {
            _logger = logger;
            var referenceTime = DateTime.UtcNow;

            var scheduledTasks = serviceProvider.GetServices<IScheduledTask>();
            foreach (var scheduledTask in scheduledTasks)
            {
                _logger.LogInformation("CRON | Schedule task {0} - {1}", scheduledTask.GetType().Name,
                    scheduledTask.Schedule);

                _scheduledTasks.Add(new SchedulerTaskWrapper(
                    schedule: CrontabSchedule.Parse(scheduledTask.Schedule),
                    task: scheduledTask,
                    nextRunTime: referenceTime
                ));
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                await ExecuteOnceAsync(cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            var referenceTime = DateTime.UtcNow;

            var tasksThatShouldRun = _scheduledTasks.Where(t => t.ShouldRun(referenceTime)).ToList();

            foreach (var taskThatShouldRun in tasksThatShouldRun)
            {
                taskThatShouldRun.Increment();
                _logger.LogInformation("CRON | {0}: Next run {1}", taskThatShouldRun.Task.GetType().Name,
                    taskThatShouldRun.NextRunTime);

                await taskFactory.StartNew(
                    async () =>
                    {
                        try
                        {
                            await taskThatShouldRun.Task.ExecuteAsync(cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            UnobservedTaskException?.Invoke(this, args);

                            if (!args.Observed)
                            {
                                throw;
                            }
                        }
                    },
                    cancellationToken);
            }
        }

        private class SchedulerTaskWrapper
        {
            public CrontabSchedule Schedule { get; private set; }
            public IScheduledTask Task { get; private set; }
            public DateTime LastRunTime { get; private set; }
            public DateTime NextRunTime { get; private set; }

            public SchedulerTaskWrapper(
                CrontabSchedule schedule,
                IScheduledTask task,
                DateTime nextRunTime)
            {
                Schedule = schedule;
                this.Task = task;
                NextRunTime = nextRunTime;
            }

            public void Increment()
            {
                LastRunTime = NextRunTime;
                NextRunTime = Schedule.GetNextOccurrence(NextRunTime);
            }

            public bool ShouldRun(DateTime currentTime)
            {
                return NextRunTime < currentTime && LastRunTime != NextRunTime;
            }
        }
    }
}
