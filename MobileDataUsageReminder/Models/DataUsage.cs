using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDataUsageReminder.Models
{
    public class DataUsage
    {
        public string PhoneNumber { get; set; }
        public int MonthlyDataGb { get; set; }
        public int DataUsedPercentage { get; set; }
        public DateTime FullDate { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }
}
