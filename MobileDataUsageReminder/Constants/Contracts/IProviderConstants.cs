namespace MobileDataUsageReminder.Constants.Contracts
{
    public interface IProviderConstants
    {
        public string PackageId { get; set; }
        public string DataTypeName { get; set; }
    }

    public interface IProviderEndpoints
    {
        public string LoginEndpoint { get; set; }
        public string ClientEndpoint { get; set; }
        public string ProductEndpoint(string clientId);
        public string DataConsumptionEndpoint(string clientId, string productId);
    }

}