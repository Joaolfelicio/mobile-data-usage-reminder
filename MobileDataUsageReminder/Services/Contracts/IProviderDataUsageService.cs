using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IProviderDataUsage
    {
        Task<List<MobileDataPackage>> GetMobileDataPackages();
    }
}
