using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp2.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20250915_164752 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComponentCode",
                table: "GradingComponentBinders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentGradeComponentCode",
                table: "ComponentApplicationInfo",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentCode",
                table: "GradingComponentBinders");

            migrationBuilder.DropColumn(
                name: "StudentGradeComponentCode",
                table: "ComponentApplicationInfo");
        }
    }
}
