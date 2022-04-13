public interface IProviderDataUsageService
{
    Task<IEnumerable<DataUsage>> GetDataUsage();
}
