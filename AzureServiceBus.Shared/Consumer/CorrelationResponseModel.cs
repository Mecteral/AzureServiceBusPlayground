namespace AzureServiceBus.Shared.Consumer
{
    public class CorrelationResponseModel
    {
        public int TotalCount { get; set; }
        public int ResponseIndex { get; set; }

        public bool IsLastRequest
            => TotalCount == ResponseIndex;
    }
}