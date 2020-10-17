using System.Collections.Generic;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Infrastructure.Contracts
{
    public interface IProviderGateway
    {
        LoginResult Login(string username, string password);

        string GetClientId();

        List<DataProduct> GetDataProducts();

        DataUsage GetDataUsage(DataProduct dataProduct);
    }
}