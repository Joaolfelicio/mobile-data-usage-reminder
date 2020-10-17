﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDataUsageReminder.Models
{
    public class MobileDataPackage
    {
        public string PhoneNumber { get; set; }
        public int ChatId { get; set; }
        public string Unit { get; set; }
        public string InitialAmount { get; set; }
        public string UsedAmount { get; set; }
        public string RemainingAmount { get; set; }
        public DateTime FullDate { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }
}
