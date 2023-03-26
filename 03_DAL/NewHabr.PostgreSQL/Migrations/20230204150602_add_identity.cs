using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewHabr.PostgreSQL.Migrations
{
    public partial class add_identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Login",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                column: "ConcurrencyStamp",
                value: "b0f79f09-2aa7-451a-a0f4-b834c0932ac1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                column: "ConcurrencyStamp",
                value: "46066405-3aff-4ebe-a9f8-d7bd0345474a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                column: "ConcurrencyStamp",
                value: "0ff2b822-82aa-472f-b65f-d7fd0cd71ba9");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "AspNetUsers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Login",
                table: "AspNetUsers",
                column: "Login");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                column: "ConcurrencyStamp",
                value: "7460a1dc-eb4f-421d-9fe9-93a22e02cf8c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                column: "ConcurrencyStamp",
                value: "91aeb3e2-440a-4ae4-bae8-c600b2372b05");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                column: "ConcurrencyStamp",
                value: "2db8706b-7b8d-4a91-afe0-e972fca2449e");
        }
    }
}
