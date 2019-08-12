using EventBus.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBus.RabbitMQ
{
    internal class RabbitMqConnectionSettings
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public int Retry { get; set; } = 5;
        public string SubscriberId { get; set; }
        public string BrokerName { get; set; } = "ESTEventBus";
    }

    public static class StartupExtension
    {
        private const string ConfigSectionName = "RabbitEventBus";

        public static IServiceCollection AddBrokerServiceConnection(this IServiceCollection service, IConfiguration configuration)
        {
            var settings = new RabbitMqConnectionSettings();
            configuration.GetSection(ConfigSectionName).Bind(settings);
            service.AddSingleton<IAmqpConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMQConnection>>();
                var connectionFactory = new ConnectionFactory
                {
                    HostName = settings.HostName,
                    Port = settings.Port,
                    UserName = settings.UserName,
                    Password = settings.Password,
                };

                return new RabbitMQConnection(connectionFactory, logger, settings.Retry);
            });
            return service;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection service, IConfiguration configuration)
        {
            var settings = new RabbitMqConnectionSettings();
            configuration.GetSection(ConfigSectionName).Bind(settings);
            service.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            service.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
                new RabbitMQEventBus(sp, settings.SubscriberId, settings.Retry, settings.BrokerName));
            return service;
        }
    }
}
