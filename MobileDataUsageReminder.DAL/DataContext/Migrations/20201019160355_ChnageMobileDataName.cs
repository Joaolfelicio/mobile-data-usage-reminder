using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MobileDataUsageReminder.DAL.DataContext.Migrations
{
    public partial class ChnageMobileDataName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileDataPackages");

            migrationBuilder.CreateTable(
                name: "MobileData",
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
                    table.PrimaryKey("PK_MobileData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileData");

            migrationBuilder.CreateTable(
                name: "MobileDataPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatId = table.Column<string>(type: "text", nullable: true),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    FullDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InitialAmount = table.Column<string>(type: "text", nullable: true),
                    Month = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    RemainingAmount = table.Column<string>(type: "text", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    UsedAmount = table.Column<string>(type: "text", nullable: true),
                    UsedPercentage = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileDataPackages", x => x.Id);
                });
        }
    }
}
