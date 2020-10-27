using System.Threading.Tasks;
using AzureServiceBus.Shared.Consumer;
using AzureServiceBus.Shared.Models;
using MassTransit;

namespace AzureServiceBus.Shared
{
    public class QueuePublisher
    {
        public static async Task StartCorrelation(string queueGuid)
        {
            var bus = Bus.Factory.CreateUsingAzureServiceBus(AzureServiceBusFactory.ConfigureHost);

            await bus.StartAsync();

            var sendEndpoint = await bus.GetSendEndpoint(Constants.QueueEndpointUri("CoreCorrelationStart"));

            await sendEndpoint.Send(new CorrelationStartRequest
            {
                QueueName = queueGuid
            });

            await bus.StopAsync();
        }

        public static async Task RespondToCorrelation(string messageQueueName)
        {
            var bus = Bus.Factory.CreateUsingAzureServiceBus(AzureServiceBusFactory.ConfigureHost);

            var sendEndpoint = await bus.GetSendEndpoint(Constants.QueueEndpointUri(messageQueueName));

            const int count = 5;

            for (var i = 0; i < count; i++)
            {
                await sendEndpoint.Send(new CorrelationResponseModel
                {
                    ResponseIndex = i,
                    TotalCount = count
                });
            }
        }

        public static async Task SendToCoreQueue(StringIntRequestModel stringIntRequestModel)
        {
            await SendToEndpoint(stringIntRequestModel, Constants.CoreWorkQueue);
        }

        public static async Task SendToFrameworkQueue(StringIntRequestModel stringIntRequestModel)
        {
            await SendToEndpoint(stringIntRequestModel, Constants.FrameWorkQueue);
        }

        private static async Task SendToEndpoint(object obj, string queueName)
        {
            var bus = Bus.Factory.CreateUsingAzureServiceBus(AzureServiceBusFactory.ConfigureHost);

            await bus.StartAsync();

            var sendEndpoint = await bus.GetSendEndpoint(Constants.QueueEndpointUri(queueName));

            await sendEndpoint.Send(obj);

            await bus.StopAsync();
        }
    }
}