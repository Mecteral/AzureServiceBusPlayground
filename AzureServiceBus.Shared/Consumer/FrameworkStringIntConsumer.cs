using System;
using System.Threading.Tasks;
using AzureServiceBus.Shared.Models;
using MassTransit;

namespace AzureServiceBus.Shared.Consumer
{
    public class FrameworkStringIntConsumer : IConsumer<StringIntRequestModel>
    {
        public async Task Consume(ConsumeContext<StringIntRequestModel> context)
            => Console.WriteLine(
                $"Framework consumer called, string: {context.Message.StringValue}, int: {context.Message.IntValue}");
    }
}