using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventBus.Contracts
{
    public interface IEventBus
    {
        void Publish(IntegratedEvent @event);

        void Subscribe<TEvent, THandler>()
            where TEvent : IntegratedEvent
            where THandler : IIntegratedEventHandler<TEvent>;

        void Unsubscribe<TEvent, THandler>()
            where TEvent : IntegratedEvent
            where THandler : IIntegratedEventHandler<TEvent>;

        void Subscribe(Type eventType, Type handlerType);
    }

    public interface IIntegratedEventHandler<in T> where T : IntegratedEvent
    {
        Task Handle(T @event);
    }
}
