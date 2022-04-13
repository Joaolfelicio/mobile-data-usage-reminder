public interface IMapperService
{
    IEnumerable<MobileData> MapMobileData(IEnumerable<DataUsage> dataUsages);
    IEnumerable<MobileData> MapMobileDataRoundUpPercent(IEnumerable<DataUsage> dataUsages);
}
