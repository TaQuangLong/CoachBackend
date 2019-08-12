using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SimpleTaskQueue.TaskQueue
{
    public class BackgroundTaskProcessingService : BackgroundService
    {
        private readonly TaskManager _taskManager;
        private readonly ILogger<BackgroundTaskProcessingService> _logger;
        private readonly IServiceProvider _serviceProvider;
        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

        public BackgroundTaskProcessingService(
            IServiceProvider serviceProvider)
        {
            _taskManager = serviceProvider.GetRequiredService<TaskManager>();
            _logger = serviceProvider.GetRequiredService<ILogger<BackgroundTaskProcessingService>>();
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            _logger.LogInformation("BackgroundTaskService is starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var job = await _taskManager.Dequeue(stoppingToken);

                await taskFactory.StartNew(
                    async () =>
                    {
                        try
                        {
                            await ExecuteOnce(job);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Exception found when execute job {0}.{1}", job.MethodInfo.DeclaringType, job.MethodInfo.Name);
                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            UnobservedTaskException?.Invoke(this, args);

                            if (!args.Observed)
                            {
                                throw;
                            }
                        }
                    },
                    stoppingToken);
            }
            _logger.LogInformation("BackgroundTaskService is stopping");
        }

        private async Task ExecuteOnce(Job job)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetServices<IPostDequeueFilter>()
                    .ToList()
                    .ForEach(t =>
                    {
                        t.OnAfterDequeue(job);
                    });

                var instance = ActivatorUtilities.GetServiceOrCreateInstance(scope.ServiceProvider, job.Type);
                if (instance is null)
                {
                    throw new InvalidOperationException($"Cannot activate type {job.Type}");
                }

                var ret = job.MethodInfo.Invoke(instance, job.Args.ToArray());

                if (ret is Task task)
                {
                    await task;
                }
            }
        }
    }
}
