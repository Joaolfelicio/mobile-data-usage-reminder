using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.DAL.DataContext
{
    public partial class MobileDataUsageContext : DbContext
    {
        public MobileDataUsageContext(DbContextOptions<MobileDataUsageContext> options)
            : base(options)
        {
        }

        public DbSet<MobileDataPackage> MobileDataPackages { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
