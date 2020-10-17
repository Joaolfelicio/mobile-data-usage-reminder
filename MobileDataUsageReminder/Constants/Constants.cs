using MobileDataUsageReminder.Constants.Contracts;

namespace MobileDataUsageReminder.Constants
{
    public class OrangeEndpoints : IOrangeEndpoints
    {
        public string LoginEndpoint { get; set; } = "https://client.orange.lu/tum/v2.0/auth/selfcare/login";
        public string ClientIdEndpoint { get; set; } = "https://client.orange.lu/tum/v2.0/users/current";
        public string ProductIdEndpoint(string clientId)
        {
            return
                $"https://client.orange.lu/bssapi/v1/customers/{clientId}/products?state=Active,Suspended,Ordered,Ready%20for%20service&limit=10000";
        }

        public string DataUsageEndpoint(string clientId, string productId)
        {
            return
                $"https://client.orange.lu/zephyr3/customers/{clientId}/usageConsumption?show=current&generateBuckets=true&productId={productId}";
        }
    }
}