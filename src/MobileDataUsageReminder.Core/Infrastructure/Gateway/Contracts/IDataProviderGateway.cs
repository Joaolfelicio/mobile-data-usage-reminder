public interface IDataProviderGateway
{
    Task<List<DataUsage>> GetDataUsages(ProviderCredentials providerCredentials, List<TelegramUser> telegramUsers);

}
