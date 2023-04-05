using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraSchool.Migrations
{
    /// <inheritdoc />
    public partial class Cascade3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Subject",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher",
                table: "Course");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Subject",
                table: "Course",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher",
                table: "Course",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Subject",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Teacher",
                table: "Course");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Subject",
                table: "Course",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher",
                table: "Course",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");
        }
    }
}
