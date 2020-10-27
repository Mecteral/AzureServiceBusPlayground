using System;
using System.Threading.Tasks;
using AzureServiceBus.Shared.Models;
using MassTransit;

namespace AzureServiceBus.Shared.Consumer
{
    public class FrameworkStringIntRpcConsumer : IConsumer<StringIntRequestModel>
    {
        public async Task Consume(ConsumeContext<StringIntRequestModel> context)
        {
            await context.RespondAsync(new StringIntResultModel
            {
                IntValue = context.Message.IntValue, StringValue = context.Message.StringValue, DateTime = DateTime.Now
            });
        }
    }
}