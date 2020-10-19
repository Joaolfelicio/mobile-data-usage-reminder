using System;
using System.Collections.Generic;
using System.Linq;
using MobileDataUsageReminder.DAL.DataContext;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.DAL.Repository.Contracts;

namespace MobileDataUsageReminder.DAL.Repository
{
    public class MobileDataRepository : IMobileDataRepository
    {
        private readonly MobileDataUsageContext _context;

        public MobileDataRepository(MobileDataUsageContext context)
        {
            _context = context;
        }

        public bool HasReminderAlreadySent(string phoneNumber, int usedPercentage)
        {
            return _context.MobileData
                .Any(x => x.Month == DateTime.Now.ToString("MMMM")
                          && x.UsedPercentage == usedPercentage
                          && x.UsedPercentage > 0
                          && x.PhoneNumber == phoneNumber);
        }

        public void CreateMobileData(MobileData mobileData)
        {
            _context.MobileData.Add(mobileData);
        }
    }
}