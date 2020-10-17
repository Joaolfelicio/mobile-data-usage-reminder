using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services.Contracts;

namespace MobileDataUsageReminder.Services
{
    public class PreviousRemindersService : IPreviousRemindersService
    {
        private readonly ILogger<PreviousRemindersService> _logger;

        public PreviousRemindersService(ILogger<PreviousRemindersService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Gets all data usages.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public List<MobileDataPackage> GetAllDataUsages(string fileName)
        {
            var dataUsages = new List<MobileDataPackage>();

            if (File.Exists(fileName))
            {
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    //Get this year records
                    var records = csv.GetRecords<MobileDataPackage>();

                    dataUsages.AddRange(records);

                    _logger.LogInformation($"Found {dataUsages.Count} records for this year.");
                }
            }
            else
            {
                _logger.LogInformation($"The file doesn't not exist or was not created yet.");
            }

            return dataUsages;
        }

        /// <summary>
        /// Gets the data usages to remind.
        /// </summary>
        /// <param name="allDataUsages">All data usages.</param>
        /// <param name="currentMobileDataPackages">The current mobile data packages.</param>
        /// <returns></returns>
        public List<MobileDataPackage> GetDataUsagesToRemind(List<MobileDataPackage> allDataUsages, List<MobileDataPackage> currentMobileDataPackages)
        {
            var dataUsagesToRemind = new List<MobileDataPackage>();

            foreach (var currentDataUsage in currentMobileDataPackages)
            {
                var reminderWasAlreadySent = allDataUsages
                    .Where(x => x.Month == DateTime.Now.ToString("MMMM"))
                    .Where(x => x.PhoneNumber == currentDataUsage.PhoneNumber)
                    .Any(x => x.UsedPercentage == currentDataUsage.UsedPercentage);

                if (reminderWasAlreadySent == false && currentDataUsage.UsedPercentage > 0)
                {
                    _logger.LogInformation($"Reminder should be sent for {currentDataUsage.PhoneNumber}: Data usage is at {currentDataUsage.UsedPercentage}% of {currentDataUsage.InitialAmount}" +
                                           $"{currentDataUsage.Unit} .");

                    dataUsagesToRemind.Add(currentDataUsage);
                }
            }

            return dataUsagesToRemind;
        }

        /// <summary>
        /// Writes all data usages.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="allDataUsages">All data usages.</param>
        public void WriteAllDataUsages(string fileName, List<MobileDataPackage> allDataUsages)
        {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(allDataUsages.OrderBy(x => x.FullDate));
            }

            _logger.LogInformation($"Deleted all records and re inserted {allDataUsages.Count} records into the record file.");
        }
    }
}