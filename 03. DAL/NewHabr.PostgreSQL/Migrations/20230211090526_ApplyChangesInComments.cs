using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewHabr.PostgreSQL.Migrations
{
    public partial class ApplyChangesInComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                column: "ConcurrencyStamp",
                value: "47190aa0-f667-4dff-8075-1bf171362f2a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                column: "ConcurrencyStamp",
                value: "1e8d0071-5be1-4df9-910b-7f454b371f66");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                column: "ConcurrencyStamp",
                value: "f5b31db5-723d-40d2-8c6d-3ebd882dd3a3");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Name",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Comments");

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
    }
}
