public interface IProviderEndpoints
{
    public string LoginEndpoint { get; set; }
    public string ClientEndpoint { get; set; }
    public string ProductEndpoint(string clientId);
    public string DataConsumptionEndpoint(string clientId, string productId);
}
