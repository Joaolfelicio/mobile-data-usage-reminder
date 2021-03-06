﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MobileDataUsageReminder.DAL.Models
{
    public class MobileData
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string ChatId { get; set; }
        public string Unit { get; set; }
        public string InitialAmount { get; set; }
        public string UsedAmount { get; set; }
        public string RemainingAmount { get; set; }
        public int UsedPercentage { get; set; }
        public DateTime FullDate { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }
}
