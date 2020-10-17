using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Infrastructure.Contracts
{
    public interface IProviderGateway
    {
        public string TokenValue { get; }
        public string TokenType { get; }
        public string ClientId { get;}

        Task Login(string username, string password);

        Task GetClient();

        Task<List<DataProduct>> GetMobileDataProducts(List<string> productsPhoneNumber);

        Task<DataUsage> GetDataUsage(DataProduct dataProduct);
    }
}