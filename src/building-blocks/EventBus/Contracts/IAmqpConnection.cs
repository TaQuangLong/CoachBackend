using RabbitMQ.Client;
using System;

namespace EventBus.Contracts
{
    public interface IAmqpConnection : IDisposable
    {
        bool IsConnected { get; }
        IModel CreateModel();
        bool TryConnect();
    }
}