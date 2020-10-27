using System.Threading.Tasks;
using MassTransit;

namespace AzureServiceBus.Shared.Consumer
{
    public class CorrelationStartConsumer : IConsumer<CorrelationStartRequest>
    {
        public async Task Consume(ConsumeContext<CorrelationStartRequest> context)
        {
            await QueuePublisher.RespondToCorrelation(context.Message.QueueName);
        }
    }
}