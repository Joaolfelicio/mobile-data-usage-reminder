using System.Collections.Generic;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IPreviousRemindersService
    {
        List<MobileDataPackage> GetAllDataUsages(string fileName);
        List<MobileDataPackage> DataUsagesToRemind(List<MobileDataPackage> allDataUsages, List<MobileDataPackage> currentMobileDataPackages);
        void WriteAllDataUsages(string fileName, List<MobileDataPackage> allDataUsages);
    }
}