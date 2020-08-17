using System.Collections.Generic;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IPreviousRemindersService
    {
        void ArchivePreviousYearReminders(string fileName);
        List<DataUsage> GetAllDataUsages(string fileName);
        List<DataUsage> DataUsagesToRemind(List<DataUsage> allDataUsages, List<DataUsage> currentDataUsages);
        void WriteAllDataUsages(string fileName, List<DataUsage> allDataUsages);
    }
}