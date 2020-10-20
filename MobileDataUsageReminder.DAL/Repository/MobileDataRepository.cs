using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.DAL.DataContext;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.DAL.Repository.Contracts;

namespace MobileDataUsageReminder.DAL.Repository
{
    public class MobileDataRepository : IMobileDataRepository
    {
        private readonly MobileDataUsageContext _context;
        private readonly ILogger<MobileDataRepository> _logger;

        public MobileDataRepository(MobileDataUsageContext context,
            ILogger<MobileDataRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public bool HasReminderAlreadySent(MobileData mobileData)
        {
            // Are there any mobile datas for the same month, with the same phone number and used %?
            return _context.MobileData
                .AsEnumerable()
                .Any(x => x.Month == DateTime.Now.ToString("MMMM") &&
                          x.PhoneNumber == mobileData.PhoneNumber &&
                          x.UsedPercentage == mobileData.UsedPercentage);
        }

        public async Task CreateMobileDatas(List<MobileData> mobileDatas)
        {
            await _context.MobileData.AddRangeAsync(mobileDatas);

            var result = await _context.SaveChangesAsync();

            _logger.LogInformation($"Saved {result} records in the database, expected: {mobileDatas.Count}");
        }
    }
}