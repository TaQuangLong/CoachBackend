namespace SimpleTaskQueue.TaskQueue
{
    public interface IPostDequeueFilter
    {
        void OnAfterDequeue(Job job);
    }
}
