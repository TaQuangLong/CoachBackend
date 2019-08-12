using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleTaskQueue.CronJobs.Scheduler;

namespace SimpleTaskQueue.CronJobs.DI
{
    public static class CronJobsSchedulerExtensions
    {
        public static IServiceCollection AddCronJobScheduler(this IServiceCollection services)
        {
            return services.AddSingleton<IHostedService, SchedulerHostedService>();
        }

        public static IServiceCollection AddCronJobScheduler(this IServiceCollection services, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler)
        {
            return services.AddSingleton<IHostedService, SchedulerHostedService>(serviceProvider =>
            {
                var logger = serviceProvider.GetService<ILogger<SchedulerHostedService>>();
                var instance = new SchedulerHostedService(serviceProvider, logger);
                instance.UnobservedTaskException += unobservedTaskExceptionHandler;
                return instance;
            });
        }
    }
}
