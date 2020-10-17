using System.Collections.Generic;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IPreviousRemindersService
    {
        void ArchivePreviousYearReminders(string fileName);
        List<MobileDataPackage> GetAllDataUsages(string fileName);
        List<MobileDataPackage> DataUsagesToRemind(List<MobileDataPackage> allDataUsages, List<MobileDataPackage> currentDataUsages);
        void WriteAllDataUsages(string fileName, List<MobileDataPackage> allDataUsages);
    }
}