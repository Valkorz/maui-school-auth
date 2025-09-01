using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp2.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20250804_100837 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComponentApplicationInfo",
                table: "ComponentApplicationInfo");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ComponentApplicationInfo",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComponentApplicationInfo",
                table: "ComponentApplicationInfo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentApplicationInfo_StudentGradeComponentId",
                table: "ComponentApplicationInfo",
                column: "StudentGradeComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComponentApplicationInfo",
                table: "ComponentApplicationInfo");

            migrationBuilder.DropIndex(
                name: "IX_ComponentApplicationInfo_StudentGradeComponentId",
                table: "ComponentApplicationInfo");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ComponentApplicationInfo",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComponentApplicationInfo",
                table: "ComponentApplicationInfo",
                columns: new[] { "StudentGradeComponentId", "Id" });
        }
    }
}
