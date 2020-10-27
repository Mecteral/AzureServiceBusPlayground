using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBus.Shared;
using AzureServiceBus.Shared.Consumer;
using MassTransit;

namespace AzureServiceBus.Core.Client
{
    internal class Program
    {
        private static int _intValue;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Press r to send requests");
            Console.WriteLine("Press x to exit console");

            char currentKey;

            do
            {
                currentKey = Console.ReadKey().KeyChar;

                if (currentKey == 'r')
                {
                    await SendRequest();
                    /*
                    IList<Task> tasks = new List<Task>();
                    for (var i = 0; i < 1; i++)
                    {
                        tasks.Add(SendRequest());
                    }

                    await Task.WhenAll(tasks);
                    */
                }
            } while (currentKey != 'x');
        }

        private static async Task SendRequest()
        {
            var semaphoreSlim = new SemaphoreSlim(0,1);

            await Task.Factory.StartNew(async () =>
            {
                // Sending the string does not work for some reason the first time around, but the second time it receives messages
                // The consumer simply does not receive any messages, maybe restart the consumer?
                var queueGuid = "CoreCorrelation".ToLower();
                var models = new List<CorrelationResponseModel>();
                var correlationConsumer = new CorrelationConsumer(model =>
                {
                    Console.WriteLine($"added model with index: {model.ResponseIndex}");
                    models.Add(model);

                    if (models.Count == model.TotalCount)
                    {
                        Console.WriteLine("Got all models in list");
                        semaphoreSlim.Release();
                    }
                });

                var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
                {
                    AzureServiceBusFactory.ConfigureHost(cfg);

                    cfg.ReceiveEndpoint(queueGuid, configurator =>
                    {
                        configurator.Consumer(() => correlationConsumer);
                    });
                });
                
                await bus.StartAsync();

                await QueuePublisher.StartCorrelation(queueGuid);

                await bus.StopAsync();
            }).ConfigureAwait(false);
          
            
            await semaphoreSlim.WaitAsync();

            /*
            var result = await RpcPublisher.SendToCoreRpcQueue(new StringIntRequestModel
            {
                IntValue = _intValue++, StringValue = "FrameworkRpcCall"
            });

            Console.WriteLine(
                $"{Environment.NewLine}string: {result.StringValue}, int: {result.IntValue}, date: {result.DateTime}{Environment.NewLine}");
            */
        }
    }
}