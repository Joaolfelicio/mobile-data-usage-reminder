namespace MobileDataUsageReminder.Constants.Contracts
{
    public interface IProviderEndpoints
    {
        public string LoginEndpoint { get; set; }
        
        public string ClientIdEndpoint { get; set; }
        public string ProductIdEndpoint(string clientId);
        public string DataUsageEndpoint(string clientId, string productId);
    }
}