using System;

namespace EventBus.Contracts
{
    public abstract class IntegratedEvent
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }

        protected IntegratedEvent()
        {
            Id = new Guid();
            Timestamp = DateTime.UtcNow;
        }
    }

    public class EventWrapper<T> where T : IntegratedEvent
    {
        public string TenantId { get; set; }
        public T Payload { get; set; }

        public EventWrapper(T payload)
        {
            Payload = payload;
        }
    }
}
