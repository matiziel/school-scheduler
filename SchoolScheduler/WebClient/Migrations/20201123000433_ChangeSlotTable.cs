using Microsoft.EntityFrameworkCore.Migrations;

namespace WebClient.Migrations
{
    public partial class ChangeSlotTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Slots_Name",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Slots");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Slots",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Slots_Index",
                table: "Slots",
                column: "Index",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Slots_Index",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Slots");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Slots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_Name",
                table: "Slots",
                column: "Name",
                unique: true);
        }
    }
}
