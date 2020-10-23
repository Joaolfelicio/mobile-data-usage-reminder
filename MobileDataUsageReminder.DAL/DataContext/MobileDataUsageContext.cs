using Microsoft.EntityFrameworkCore;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.DAL.DataContext
{
    public class MobileDataUsageContext : DbContext
    {
        public MobileDataUsageContext()
        {
            
        }
        public MobileDataUsageContext(DbContextOptions<MobileDataUsageContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(
                "Server=localhost;Port=5432;Database=MobileDataUsage;User Id=postgres;Password=password;");
        }

        public DbSet<MobileData> MobileData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
