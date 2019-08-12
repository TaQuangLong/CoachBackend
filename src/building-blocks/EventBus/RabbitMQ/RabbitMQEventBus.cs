using EventBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//using Dgm.Core.Extensions;
using Polly;
//using Tenants.Common.Context;

namespace EventBus.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        private readonly string _brokerName;
        private readonly IAmqpConnection _persistentConnection;
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly int _retryCount;
        private readonly IServiceProvider _serviceProvider;

        private IModel _consumerChannel;
        private string _queueName;
        //private readonly TenantContextAccessor _tenantContextAccessor;
        //private readonly ITenantContextFactory _tenantContextFactory;

        public RabbitMQEventBus(
            IServiceProvider serviceProvider,
            string queueName = null,
            int retryCount = 5,
            string brokerName = "est_event_bus")
        {
            _serviceProvider = serviceProvider;
            _persistentConnection = serviceProvider.GetRequiredService<IAmqpConnection>();
            _logger = _serviceProvider.GetService<ILogger<RabbitMQEventBus>>();
            _subsManager = serviceProvider.GetRequiredService<IEventBusSubscriptionsManager>();
            //_tenantContextAccessor = serviceProvider.GetRequiredService<TenantContextAccessor>();
            //_tenantContextFactory = serviceProvider.GetRequiredService<ITenantContextFactory>();
            _queueName = queueName;
            _brokerName = brokerName;
            _retryCount = retryCount;
            _consumerChannel = CreateConsumerChannel();
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: _queueName,
                    exchange: _brokerName,
                    routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }

        public void Publish(IntegratedEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                });

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = @event.GetType()
                    .Name;

                channel.ExchangeDeclare(exchange: _brokerName,
                                    type: "direct");

                var package = TryWrap(@event);
                //var message = JsonConvert.SerializeObject(package);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent
                    channel.BasicPublish(exchange: _brokerName,
                                     routingKey: eventName,
                                     mandatory: true,
                                     basicProperties: properties,
                                     body: body);
                });
            }
        }

        private EventWrapper<T> TryWrap<T>(T @event) where T : IntegratedEvent
        {
            //var context = _tenantContextAccessor.TenantContext;
            var package = new EventWrapper<T>(@event)
            {
                TenantId = context?.TenantId
            };
            return package;
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IntegratedEvent
            where THandler : IIntegratedEventHandler<TEvent>
        {
            var eventName = _subsManager.GetEventKey<TEvent>();
            DoInternalSubscription(eventName);
            _subsManager.AddSubscription<TEvent, THandler>();
        }

        public void Subscribe(Type eventType, Type handlerType)
        {
            if (!typeof(IntegratedEvent).IsAssignableFrom(eventType))
            {
                throw new ArgumentOutOfRangeException(nameof(eventType),
                    "Event type must inherit from 'IntegratedEvent'");
            }

            //if (!typeof(IIntegratedEventHandler<>).IsAssignableFromGeneric(handlerType))
            {
                throw new ArgumentOutOfRangeException(nameof(handlerType),
                    "Event type must inherit from 'IIntegratedEventHandler<>'");
            }

            var eventName = _subsManager.GetEventKey(eventType);
            DoInternalSubscription(eventName);
            _subsManager.AddSubscription(eventType, handlerType);
        }


        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: _queueName,
                                      exchange: _brokerName,
                                      routingKey: eventName);
                }
            }
        }

        public void Unsubscribe<TEvent, THandler>()
            where THandler : IIntegratedEventHandler<TEvent>
            where TEvent : IntegratedEvent
        {
            _subsManager.RemoveSubscription<TEvent, THandler>();
        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();

            _subsManager.Clear();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _brokerName,
                                 type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);
                try
                {
                    await ProcessEvent(eventName, message);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, ex.Message, ex.StackTrace);
                }

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var eventType = _subsManager.GetEventTypeByName(eventName);
                if (eventType == null)
                {
                    _logger.LogWarning("Unregistered event {0}", eventName);
                    return;
                }

                var (context, payload) = await Unwrap(message, eventType);
                if (payload == null)
                {
                    _logger.LogError("Integrated event malformed. Payload: {0}", message);
                    return;
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (context != null)
                        {
                            var contextAccessor = scope.ServiceProvider.GetService<TenantContextAccessor>();
                            contextAccessor.TenantContext = context;
                        }

                        var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                        var concreteType = typeof(IIntegratedEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new[] { payload });
                    }
                }
            }
        }

        /// <summary>
        /// assign context
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        //private async Task<(TenantContext, object)> Unwrap(string message, Type eventType)
        //{
        //    var wrappedType = typeof(EventWrapper<>).MakeGenericType(eventType);
        //    try
        //    {
        //        var package = JsonConvert.DeserializeObject(message, wrappedType);

        //        if (package == null)
        //        {
        //            return (null, null);
        //        }

        //        var tenantId = wrappedType
        //            .GetProperty("TenantId")?
        //            .GetValue(package)?
        //            .ToString();

        //        var payload = wrappedType.GetProperty("Payload").GetValue(package);
        //        var context = string.IsNullOrEmpty(tenantId) ? null : await _tenantContextFactory.CreateForId(tenantId);
        //        return (context, payload);
        //    }
        //    catch (JsonSerializationException e)
        //    {
        //        return (null, null);
        //    }
        //}
    }
}
