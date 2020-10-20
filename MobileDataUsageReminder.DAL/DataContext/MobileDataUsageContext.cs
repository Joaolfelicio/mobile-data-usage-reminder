using Microsoft.EntityFrameworkCore;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.DAL.DataContext
{
    public partial class MobileDataUsageContext : DbContext
    {
        public MobileDataUsageContext()
        {
        }

        public MobileDataUsageContext(DbContextOptions<MobileDataUsageContext> options)
            : base(options)
        {
        }

        public DbSet<MobileData> MobileData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
