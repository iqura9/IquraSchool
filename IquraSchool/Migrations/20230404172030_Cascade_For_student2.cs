using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraSchool.Migrations
{
    /// <inheritdoc />
    public partial class Cascade_For_student2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Student",
                table: "Grade");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Student",
                table: "Grade",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Student",
                table: "Grade");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Student",
                table: "Grade",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id");
        }
    }
}
