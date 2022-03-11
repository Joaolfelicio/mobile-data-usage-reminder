public interface IProviderEndpoints
{
    public string LoginEndpoint { get; }
    public string ClientEndpoint { get; }
    public string ProductEndpoint(string clientId);
    public string DataConsumptionEndpoint(string clientId, string productId);
}
