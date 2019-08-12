using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleTaskQueue.TaskQueue
{
    public static class BackgroundTaskAspnetExtension
    {
        public static void AddSimpleTaskQueue(this IServiceCollection services,
            EventHandler<UnobservedTaskExceptionEventArgs> onException = null)
        {
            services.AddSingleton(sp =>
            {
                var instance = new TaskManager(sp);
                BackgroundJob.SetTaskManager(instance);
                return instance;
            });

            services.AddSingleton<IHostedService, BackgroundTaskProcessingService>(sp =>
            {
                var instance = new BackgroundTaskProcessingService(sp);

                if (onException != null)
                {
                    instance.UnobservedTaskException += onException;
                }

                return instance;
            });
        }
    }
}
