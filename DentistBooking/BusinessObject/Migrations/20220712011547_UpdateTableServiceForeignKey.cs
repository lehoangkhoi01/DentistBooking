using Microsoft.EntityFrameworkCore.Migrations;

namespace BusinessObject.Migrations
{
    public partial class UpdateTableServiceForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Admins_AdminId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_AdminId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Services");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CreatedPersonId",
                table: "Services",
                column: "CreatedPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Admins_CreatedPersonId",
                table: "Services",
                column: "CreatedPersonId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Admins_CreatedPersonId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_CreatedPersonId",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_AdminId",
                table: "Services",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Admins_AdminId",
                table: "Services",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
