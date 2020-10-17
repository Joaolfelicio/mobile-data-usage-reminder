using System.Collections.Generic;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IPreviousRemindersService
    {
        /// <summary>
        /// Gets all data usages.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        List<MobileDataPackage> GetAllDataUsages(string fileName);

        /// <summary>
        /// Gets the data usages to remind.
        /// </summary>
        /// <param name="allDataUsages">All data usages.</param>
        /// <param name="currentMobileDataPackages">The current mobile data packages.</param>
        /// <returns></returns>
        List<MobileDataPackage> GetDataUsagesToRemind(List<MobileDataPackage> allDataUsages, List<MobileDataPackage> currentMobileDataPackages);

        /// <summary>
        /// Writes all data usages.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="allDataUsages">All data usages.</param>
        void WriteAllDataUsages(string fileName, List<MobileDataPackage> allDataUsages);
    }
}