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
}