using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleTaskQueue.TaskQueue
{
    public class TaskManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<Job> _queue = new ConcurrentQueue<Job>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public TaskManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Enqueue<T>(Expression<Func<T, Task>> methodCall)
        {
            var job = Job.FromExpression(methodCall);
            Enqueue(job);
        }

        public void Enqueue(Expression<Func<Task>> methodCall)
        {
            var job = Job.FromExpression(methodCall);
            Enqueue(job);
        }

        public void Enqueue(Expression<Action> methodCall)
        {
            var job =  Job.FromExpression(methodCall);
            Enqueue(job);
        }

        public void Enqueue<T>(Expression<Action<T>> methodCall)
        {
            var job = Job.FromExpression(methodCall);
            Enqueue(job);
        }

        private void Enqueue(Job job)
        {
            _serviceProvider.GetServices<IPreEnqueueFilter>().ToList()
                .ForEach(t =>
                {
                    t.OnBeforeEnqueue(job);
                });
            _signal.Release();
            _queue.Enqueue(job);
        }

        internal async Task<Job> Dequeue(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var job);
            return job;
        }
    }
}
