namespace SimpleTaskQueue.TaskQueue
{
    public interface IPreEnqueueFilter
    {
        void OnBeforeEnqueue(Job job);
    }
}
