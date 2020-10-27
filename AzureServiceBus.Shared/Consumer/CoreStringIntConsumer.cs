using System;
using System.Threading.Tasks;
using AzureServiceBus.Shared.Models;
using MassTransit;

namespace AzureServiceBus.Shared.Consumer
{
    public class CoreStringIntConsumer : IConsumer<StringIntRequestModel>
    {
        public async Task Consume(ConsumeContext<StringIntRequestModel> context)
            => Console.WriteLine(
                $"Core consumer called, string: {context.Message.StringValue}, int: {context.Message.IntValue}");
    }

    public interface ITest
    {

    }

    public class Test : ITest
    {

    }
}