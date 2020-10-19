using System;
using System.Collections.Generic;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.DAL.Repository.Contracts
{
    public interface IMobileDataRepository
    {
        bool HasReminderAlreadySent(string phoneNumber, int usedPercentage);

        void CreateMobileData(MobileData mobileData);
    }
}