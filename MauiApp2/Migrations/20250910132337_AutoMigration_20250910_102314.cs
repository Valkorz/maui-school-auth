using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp2.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20250910_102314 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradingComponentBinder_Users_UserId",
                table: "GradingComponentBinder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradingComponentBinder",
                table: "GradingComponentBinder");

            migrationBuilder.RenameTable(
                name: "GradingComponentBinder",
                newName: "GradingComponentBinders");

            migrationBuilder.RenameIndex(
                name: "IX_GradingComponentBinder_UserId",
                table: "GradingComponentBinders",
                newName: "IX_GradingComponentBinders_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "StudentGradeComponentId",
                table: "ComponentApplicationInfo",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "GradingComponentBinders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradingComponentBinders",
                table: "GradingComponentBinders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradingComponentBinders_Users_UserId",
                table: "GradingComponentBinders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradingComponentBinders_Users_UserId",
                table: "GradingComponentBinders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradingComponentBinders",
                table: "GradingComponentBinders");

            migrationBuilder.RenameTable(
                name: "GradingComponentBinders",
                newName: "GradingComponentBinder");

            migrationBuilder.RenameIndex(
                name: "IX_GradingComponentBinders_UserId",
                table: "GradingComponentBinder",
                newName: "IX_GradingComponentBinder_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "StudentGradeComponentId",
                table: "ComponentApplicationInfo",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "GradingComponentBinder",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradingComponentBinder",
                table: "GradingComponentBinder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradingComponentBinder_Users_UserId",
                table: "GradingComponentBinder",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
