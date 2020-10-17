using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Infrastructure.Contracts
{
    public interface IProviderGateway
    {

        public string TokenValue { get; }
        public string TokenType { get; }
        
        /// <summary>
        /// Provider client Id to send the reminder to
        /// </summary>
        public string ClientId { get;}

        /// <summary>
        /// Login to the provider, will store the TokenValue and the TokenType
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        Task Login(string username, string password);

        /// <summary>
        /// Get the client details and store the client id
        /// </summary>
        Task GetClient();

        /// <summary>
        /// Gets the data products to the phone numbers
        /// </summary>
        /// <param name="productsPhoneNumber"></param>
        /// <returns>List of the data products</returns>
        Task<List<DataProduct>> GetMobileDataProducts(List<string> productsPhoneNumber);

        /// <summary>
        /// Gets the data usage for the data product
        /// </summary>
        /// <param name="dataProduct"></param>
        /// <returns>The data usage</returns>
        Task<DataUsage> GetDataUsage(DataProduct dataProduct);
    }
}