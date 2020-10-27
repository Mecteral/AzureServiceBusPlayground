using System;
using System.Threading.Tasks;
using MassTransit;

namespace AzureServiceBus.Shared.Consumer
{
    public class CorrelationConsumer : IConsumer<CorrelationResponseModel>
    {
        private readonly Action<CorrelationResponseModel> consumeAction;

        public CorrelationConsumer(Action<CorrelationResponseModel> consumeAction)
        {
            this.consumeAction = consumeAction ?? throw new ArgumentNullException(nameof(consumeAction));
        }

        public Task Consume(ConsumeContext<CorrelationResponseModel> context)
        {
            consumeAction(context.Message);

            return Task.CompletedTask;
        }
    }
}