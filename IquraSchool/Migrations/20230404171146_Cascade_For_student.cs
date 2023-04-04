using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraSchool.Migrations
{
    /// <inheritdoc />
    public partial class Cascade_For_student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Info_Group",
                table: "Schedule_Info");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Group",
                table: "Student");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Info_Group",
                table: "Schedule_Info",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Group",
                table: "Student",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Info_Group",
                table: "Schedule_Info");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Group",
                table: "Student");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Info_Group",
                table: "Schedule_Info",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Group",
                table: "Student",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id");
        }
    }
}
