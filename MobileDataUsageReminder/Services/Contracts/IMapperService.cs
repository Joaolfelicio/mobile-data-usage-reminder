using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IMapperService
    {
        MobileData MapMobileData(DataUsage dataUsage);
    }
}
