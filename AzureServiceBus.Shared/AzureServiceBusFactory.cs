using AzureServiceBus.Shared.Consumer;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;

namespace AzureServiceBus.Shared
{
    public class AzureServiceBusFactory
    {
        public static void ConfigureHost(IServiceBusBusFactoryConfigurator cfg)
        {
            //cfg.HostEndpoint(new Uri($"rabbitmq://{Constants.RabbitUserName}:{Constants.RabbitPassword}@{Constants.HostEndpoint}:{Constants.RabbitPort}/{Constants.RabbitVirtualNetwork}"));

            cfg.Host(Constants.HostEndpoint);
        }

        public static void ConfigureCoreWebServices(IServiceBusBusFactoryConfigurator configurator,
            IBusRegistrationContext context)
        {
            ConfigureHost(configurator);

            configurator.ReceiveEndpoint(Constants.CoreRpcQueue, e =>
            {
                e.ConfigureConsumer<CoreStringIntRpcConsumer>(context);
            });

            configurator.ReceiveEndpoint(Constants.CoreWorkQueue, e =>
            {
                e.ConfigureConsumer<CoreStringIntConsumer>(context);
            });

            configurator.ReceiveEndpoint("CoreCorrelationStart", e =>
            {
                e.ConfigureConsumer<CorrelationStartConsumer>(context);
            });
        }

        public static void ConfigureFrameworkWebServices(IServiceBusBusFactoryConfigurator configurator)
        {
            ConfigureHost(configurator);

            configurator.ReceiveEndpoint(Constants.FrameWorkQueue, e =>
            {
                e.Consumer<FrameworkStringIntConsumer>();
            });

            configurator.ReceiveEndpoint(Constants.FrameWorkRpcQueue, e =>
            {
                e.Consumer<FrameworkStringIntRpcConsumer>();
            });
        }
    }
}