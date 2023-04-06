using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraSchool.Migrations
{
    /// <inheritdoc />
    public partial class MigrationXF : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Course",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Info_Course",
                table: "Schedule_Info");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Course",
                table: "Grade",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Info_Course",
                table: "Schedule_Info",
                column: "CourseId",
                principalTable: "Course",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Course",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Info_Course",
                table: "Schedule_Info");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Course",
                table: "Grade",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Info_Course",
                table: "Schedule_Info",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }
    }
}
