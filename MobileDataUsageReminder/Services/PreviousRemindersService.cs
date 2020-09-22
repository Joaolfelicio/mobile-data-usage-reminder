using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services.Contracts;

namespace MobileDataUsageReminder.Services
{
    public class PreviousRemindersService : IPreviousRemindersService
    {
        public void ArchivePreviousYearReminders(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var firstRecord = csv.GetRecords<DataUsage>().FirstOrDefault();

                    //If the file holds the records for the previous year, delete it
                    if (firstRecord?.Year != DateTime.Now.Year)
                    {
                        File.Delete(fileName);
                    }
                }
            }
        }

        public List<DataUsage> GetAllDataUsages(string fileName)
        {
            var dataUsages = new List<DataUsage>();

            if (File.Exists(fileName))
            {
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    //Get this month records
                    var records = csv.GetRecords<DataUsage>();

                    dataUsages.AddRange(records);
                }
            }

            return dataUsages;
        }

        public List<DataUsage> DataUsagesToRemind(List<DataUsage> allDataUsages, List<DataUsage> currentDataUsages)
        {
            var dataUsagesToRemind = new List<DataUsage>();

            foreach (var currentDataUsage in currentDataUsages)
            {
                var reminderWasAlreadySent = allDataUsages
                    .Where(x => x.Month == DateTime.Now.ToString("MMMM"))
                    .Where(x => x.PhoneNumber == currentDataUsage.PhoneNumber)
                    .Any(x => x.DataUsedPercentage == currentDataUsage.DataUsedPercentage);

                if (reminderWasAlreadySent == false)
                {
                    dataUsagesToRemind.Add(currentDataUsage);
                }
            }

            return dataUsagesToRemind;
        }

        public void WriteAllDataUsages(string fileName, List<DataUsage> allDataUsages)
        {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(allDataUsages.OrderBy(x => x.FullDate));
            }
        }
    }
}