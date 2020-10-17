using MobileDataUsageReminder.Constants.Contracts;

namespace MobileDataUsageReminder.Constants
{
    public class OrangeConstants : IOrangeConstants
    {
        public string PackageId { get; set; } = "OfferMALike";
        public string DataTypeName { get; set; } = "National and Europe Data";
    }

    public class OrangeEndpoints : IOrangeEndpoints
    {
        public string LoginEndpoint { get; set; } = "https://client.orange.lu/tum/v2.0/auth/selfcare/login";
        public string ClientEndpoint { get; set; } = "https://client.orange.lu/tum/v2.0/users/current";
        public string ProductEndpoint(string clientId)
        {
            return
                $"https://client.orange.lu/bssapi/v1/customers/{clientId}/products?state=Active,Suspended,Ordered,Ready%20for%20service&limit=10000";
        }

        public string DataConsumptionEndpoint(string clientId, string productId)
        {
            return
                $"https://client.orange.lu/zephyr3/customers/{clientId}/usageConsumption?show=current&generateBuckets=true&productId={productId}";
        }
    }
}