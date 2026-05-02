using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalorieTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CalorieTarget = table.Column<double>(type: "REAL", nullable: false),
                    ProteinTargetG = table.Column<double>(type: "REAL", nullable: false),
                    CarbsTargetG = table.Column<double>(type: "REAL", nullable: false),
                    FatTargetG = table.Column<double>(type: "REAL", nullable: false),
                    FiberTargetG = table.Column<double>(type: "REAL", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyGoals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiaryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MealType = table.Column<string>(type: "TEXT", nullable: false),
                    FoodName = table.Column<string>(type: "TEXT", nullable: false),
                    FoodBarcode = table.Column<string>(type: "TEXT", nullable: true),
                    QuantityGrams = table.Column<double>(type: "REAL", nullable: false),
                    Calories = table.Column<double>(type: "REAL", nullable: false),
                    ProteinG = table.Column<double>(type: "REAL", nullable: false),
                    CarbsG = table.Column<double>(type: "REAL", nullable: false),
                    FatG = table.Column<double>(type: "REAL", nullable: false),
                    FiberG = table.Column<double>(type: "REAL", nullable: false),
                    SugarG = table.Column<double>(type: "REAL", nullable: false),
                    VitaminCMg = table.Column<double>(type: "REAL", nullable: true),
                    VitaminAMcg = table.Column<double>(type: "REAL", nullable: true),
                    CalciumMg = table.Column<double>(type: "REAL", nullable: true),
                    IronMg = table.Column<double>(type: "REAL", nullable: true),
                    SodiumMg = table.Column<double>(type: "REAL", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryEntries", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DailyGoals",
                columns: new[] { "Id", "CalorieTarget", "CarbsTargetG", "FatTargetG", "FiberTargetG", "ProteinTargetG", "UpdatedAt" },
                values: new object[] { 1, 2000.0, 250.0, 65.0, 30.0, 150.0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyGoals");

            migrationBuilder.DropTable(
                name: "DiaryEntries");
        }
    }
}
