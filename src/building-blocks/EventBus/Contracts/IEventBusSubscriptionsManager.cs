using System;
using System.Collections.Generic;

namespace EventBus.Contracts
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>()
            where T : IntegratedEvent
            where TH : IIntegratedEventHandler<T>;

        void RemoveSubscription<T, TH>()
            where TH : IIntegratedEventHandler<T>
            where T : IntegratedEvent;

        bool HasSubscriptionsForEvent<T>() where T : IntegratedEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegratedEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<T>();
        string GetEventKey(Type t);

        void AddSubscription(Type eventType, Type handlerType);
    }

    public class SubscriptionInfo
    {
        public Type HandlerType{ get; }

        public SubscriptionInfo(Type handlerType)
        {
            HandlerType = handlerType;
        }
    }
}
