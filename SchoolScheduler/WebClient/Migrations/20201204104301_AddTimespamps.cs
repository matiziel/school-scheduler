using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebClient.Migrations
{
    public partial class AddTimespamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Teachers",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Subjects",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Slots",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Rooms",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "ClassGroups",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Activities",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "ClassGroups");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Activities");
        }
    }
}
