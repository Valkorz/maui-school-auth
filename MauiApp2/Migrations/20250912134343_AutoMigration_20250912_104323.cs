using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp2.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20250912_104323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudentGradeComponentId",
                table: "ComponentApplicationInfo",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudentGradeComponentId",
                table: "ComponentApplicationInfo",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
