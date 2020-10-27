using System;

namespace AzureServiceBus.Shared
{
    public static class Constants
    {
        public const string FrameWorkQueue = "FrameworkQueueEndpoint";
        public const string CoreWorkQueue = "CoreQueueEndpoint";
        public const string FrameWorkRpcQueue = "FrameworkRpcQueueEndpoint";
        public const string CoreRpcQueue = "CoreRpcQueueEndpoint";

        private const string HostAddress = "haiprotest.servicebus.windows.net";
        private const string SharedAccessKeyName = "RootManageSharedAccessKey";
        private const string SharedAccessKey = "xGRJFo76/og/GybwepHjZ44ITd0tTCgo2LtyKa7z5XQ=";
        public static readonly string HostEndpoint = $"Endpoint=sb://{HostAddress}/;SharedAccessKeyName={SharedAccessKeyName};SharedAccessKey={SharedAccessKey}";

        public static Uri QueueEndpointUri (string queueName)
            => new Uri($"sb://{HostAddress}/{queueName}");
    }
}