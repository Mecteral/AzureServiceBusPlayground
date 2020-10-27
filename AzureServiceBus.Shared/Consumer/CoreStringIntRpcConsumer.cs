using System;
using System.Threading.Tasks;
using AzureServiceBus.Shared.Models;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Context;

namespace AzureServiceBus.Shared.Consumer
{
    public class CoreStringIntRpcConsumer : IConsumer<StringIntRequestModel>
    {
        private readonly ITest test;

        public CoreStringIntRpcConsumer(ITest test)
        {
            this.test = test;
        }

        public async Task Consume(ConsumeContext<StringIntRequestModel> context)
        {
            await context.RespondAsync(new StringIntResultModel
            {
                IntValue = test.GetHashCode(), StringValue = context.Message.StringValue, DateTime = DateTime.Now
            }, sendContext => sendContext.SetSessionId(context.SessionId()));

            await context.RespondAsync(new StringIntResultModel
            {
                IntValue = test.GetHashCode(),
                StringValue = context.Message.StringValue,
                DateTime = DateTime.Now
            });
        }
    }

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

    public class CorrelationStartConsumer : IConsumer<CorrelationStartRequest>
    {
        public async Task Consume(ConsumeContext<CorrelationStartRequest> context)
        {
            await QueuePublisher.RespondToCorrelation();
        }
    }

    public class CorrelationStartRequest
    {

    }

    public class CorrelationResponseModel
    {
        public int TotalCount { get; set; }
        public int ResponseIndex { get; set; }

        public bool IsLastRequest
            => TotalCount == ResponseIndex;
    }
}