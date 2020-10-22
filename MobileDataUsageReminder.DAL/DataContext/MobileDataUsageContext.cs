using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.DAL.DataContext
{
    public class MobileDataUsageContext : DbContext
    {
        public MobileDataUsageContext(DbContextOptions<MobileDataUsageContext> options)
            : base(options)
        { }

        public DbSet<MobileData> MobileData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
