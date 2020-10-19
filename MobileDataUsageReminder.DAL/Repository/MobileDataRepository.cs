using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> HasReminderAlreadySent(MobileData mobileData)
        {
            var mobileDatas = await _context.MobileData
                .ToListAsync();
            
            // See if there is mobile 
            return mobileDatas
                    .Any(y => y.UsedPercentage == mobileData.UsedPercentage
                                        && y.PhoneNumber == mobileData.PhoneNumber
                                        && y.Month == DateTime.Now.ToString("MMMM")
                                        && y.UsedPercentage == mobileData.UsedPercentage);
        }

        public async Task CreateMobileDatas(List<MobileData> mobileDatas)
        {
            await _context.MobileData.AddRangeAsync(mobileDatas);

            var result = await _context.SaveChangesAsync();

            _logger.LogInformation($"Saved {result} records in the database, expected: {mobileDatas.Count}");
        }
    }
}