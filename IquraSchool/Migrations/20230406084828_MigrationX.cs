using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraSchool.Migrations
{
    /// <inheritdoc />
    public partial class MigrationX : Migration
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
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Teacher",
                table: "Course",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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
    }
}
