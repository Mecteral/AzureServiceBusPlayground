using System;
using System.Threading.Tasks;
using AzureServiceBus.Shared.Consumer;
using AzureServiceBus.Shared.Models;
using MassTransit;

namespace AzureServiceBus.Shared
{
    public class RpcPublisher
    {
        public static async Task<StringIntResultModel> SendToCoreRpcQueue(StringIntRequestModel stringIntRequestModel)
            => await SendRpcCall(stringIntRequestModel, Constants.CoreRpcQueue);

        public static async Task<StringIntResultModel> SendToFrameworkRpcQueue(
            StringIntRequestModel stringIntRequestModel)
            => await SendRpcCall(stringIntRequestModel, Constants.FrameWorkRpcQueue);

        private static async Task<StringIntResultModel> SendRpcCall(StringIntRequestModel stringIntRequestModel,
            string queueName)
        {
            try
            {
                var bus = Bus.Factory.CreateUsingAzureServiceBus(AzureServiceBusFactory.ConfigureHost);

                await bus.StartAsync();

                var client =
                    bus.CreateRequestClient<StringIntRequestModel>(Constants.QueueEndpointUri(queueName));

                var response = await client.GetResponse<StringIntResultModel>(stringIntRequestModel, default, RequestTimeout.After(0,0,5));

                await bus.StopAsync();

                return response.Message;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}