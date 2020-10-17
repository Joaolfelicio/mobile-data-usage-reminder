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
        public void ArchivePreviousYearReminders(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var firstRecord = csv.GetRecords<MobileDataPackage>().FirstOrDefault();

                    //If the file holds the records for the previous year, delete it
                    if (firstRecord?.Year != DateTime.Now.Year)
                    {
                        _logger.LogInformation($"Deleting previous year file for the year of {firstRecord?.Year}.");

                        File.Delete(fileName);
                    }
                    else
                    {
                        _logger.LogInformation($"No file was deleted as the file is for the current year of {firstRecord?.Year}.");
                    }
                }
            }
        }

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

        public List<MobileDataPackage> DataUsagesToRemind(List<MobileDataPackage> allDataUsages, List<MobileDataPackage> currentDataUsages)
        {
            var dataUsagesToRemind = new List<MobileDataPackage>();

            foreach (var currentDataUsage in currentDataUsages)
            {
                var reminderWasAlreadySent = allDataUsages
                    .Where(x => x.Month == DateTime.Now.ToString("MMMM"))
                    .Where(x => x.PhoneNumber == currentDataUsage.PhoneNumber)
                    .Any(x => x.DataUsedPercentage == currentDataUsage.DataUsedPercentage);

                if (reminderWasAlreadySent == false)
                {
                    _logger.LogInformation($"Reminder should be sent for: Data usage is at {currentDataUsage.DataUsedPercentage}% of {currentDataUsage.MonthlyDataGb}GB" +
                                           $"for number {currentDataUsage.PhoneNumber}.");

                    dataUsagesToRemind.Add(currentDataUsage);
                }
            }

            return dataUsagesToRemind;
        }

        public void WriteAllDataUsages(string fileName, List<MobileDataPackage> allDataUsages)
        {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(allDataUsages.OrderBy(x => x.FullDate));
            }

            _logger.LogInformation($"Deleted all records and re inserted {allDataUsages.Count} records into this year file.");
        }
    }
}