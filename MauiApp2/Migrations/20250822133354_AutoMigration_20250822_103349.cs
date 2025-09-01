using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp2.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20250822_103349 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Period",
                table: "GradingComponentBinder",
                newName: "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "Identification",
                table: "GradingComponentBinder",
                newName: "PeriodEnd");

            migrationBuilder.RenameColumn(
                name: "Period",
                table: "ComponentApplicationInfo",
                newName: "PeriodStart");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PeriodEnd",
                table: "ComponentApplicationInfo",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeriodEnd",
                table: "ComponentApplicationInfo");

            migrationBuilder.RenameColumn(
                name: "PeriodStart",
                table: "GradingComponentBinder",
                newName: "Period");

            migrationBuilder.RenameColumn(
                name: "PeriodEnd",
                table: "GradingComponentBinder",
                newName: "Identification");

            migrationBuilder.RenameColumn(
                name: "PeriodStart",
                table: "ComponentApplicationInfo",
                newName: "Period");
        }
    }
}
