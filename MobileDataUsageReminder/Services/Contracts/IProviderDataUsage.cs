using System;
using System.Collections.Generic;
using System.Text;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IProviderDataUsage
    {
        List<MobileDataPackage> GetMobileDataPackages();
    }
}
