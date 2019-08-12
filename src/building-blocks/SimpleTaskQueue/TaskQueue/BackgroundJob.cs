using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleTaskQueue.TaskQueue
{
    // Handy static method
    public static class BackgroundJob
    {
        private static TaskManager _taskManagerInstance;

        internal static void SetTaskManager(TaskManager taskManager)
        {
            _taskManagerInstance = taskManager;
        }

        public static void Enqueue<T>(Expression<Func<T, Task>> methodCall)
        {
            _taskManagerInstance.Enqueue(methodCall);
        }

        public static void Enqueue(Expression<Func<Task>> methodCall)
        {
            _taskManagerInstance.Enqueue(methodCall);
        }

        public static void Enqueue(Expression<Action> methodCall)
        {
            _taskManagerInstance.Enqueue(methodCall);
        }

        public static void Enqueue<T>(Expression<Action<T>> methodCall)
        {
            _taskManagerInstance.Enqueue(methodCall);
        }
    }
}
