using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MobileDataUsageReminder.DAL.DataContext.Migrations
{
    public partial class CreateMobileDataPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MobileDataPackages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ChatId = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    InitialAmount = table.Column<string>(nullable: true),
                    UsedAmount = table.Column<string>(nullable: true),
                    RemainingAmount = table.Column<string>(nullable: true),
                    UsedPercentage = table.Column<int>(nullable: false),
                    FullDate = table.Column<DateTime>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Month = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileDataPackages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileDataPackages");
        }
    }
}
