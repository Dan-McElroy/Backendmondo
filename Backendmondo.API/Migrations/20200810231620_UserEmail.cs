using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backendmondo.API.Migrations
{
    public partial class UserEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ended",
                table: "SubscriptionPauses",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ended",
                table: "SubscriptionPauses",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
