using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.Configurations;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Infrastructure.Contracts
{
    public interface IProviderGateway
    {
        Task<List<DataUsage>> GetDataUsages(ProviderCredentials providerCredentials, List<TelegramUser> telegramUsers);
    }
}