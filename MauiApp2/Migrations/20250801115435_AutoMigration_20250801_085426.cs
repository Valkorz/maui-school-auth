using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiApp2.Migrations
{
    /// <inheritdoc />
    public partial class AutoMigration_20250801_085426 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Semester = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetCourses = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentApplicationInfo",
                columns: table => new
                {
                    StudentGradeComponentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Identification = table.Column<string>(type: "TEXT", nullable: false),
                    Classroom = table.Column<string>(type: "TEXT", nullable: false),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    Period = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentApplicationInfo", x => new { x.StudentGradeComponentId, x.Id });
                    table.ForeignKey(
                        name: "FK_ComponentApplicationInfo_Components_StudentGradeComponentId",
                        column: x => x.StudentGradeComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentApplicationInfo");

            migrationBuilder.DropTable(
                name: "Components");
        }
    }
}
