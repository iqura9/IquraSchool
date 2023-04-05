using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IquraSchool.Migrations
{
    /// <inheritdoc />
    public partial class Cascade5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Teacher",
                table: "Group");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Teacher",
                table: "Group",
                column: "HeadTeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Teacher",
                table: "Group");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Teacher",
                table: "Group",
                column: "HeadTeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
