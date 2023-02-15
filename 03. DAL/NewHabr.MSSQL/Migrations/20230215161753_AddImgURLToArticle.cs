using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewHabr.MSSQL.Migrations
{
    public partial class AddImgURLToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId",
                table: "UserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserNotifications");

            migrationBuilder.AddColumn<string>(
                name: "ImgURL",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationUser",
                columns: table => new
                {
                    NotificationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationUser", x => new { x.NotificationsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_NotificationUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationUser_UserNotifications_NotificationsId",
                        column: x => x.NotificationsId,
                        principalTable: "UserNotifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                column: "ConcurrencyStamp",
                value: "00f63f30-9255-4ec6-9503-a8ec8191c744");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                column: "ConcurrencyStamp",
                value: "9f7a2799-5283-46e5-83cf-b6251a3d4fe4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                column: "ConcurrencyStamp",
                value: "91eebc45-08d9-4dc4-badb-b70fc1e57da8");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUser_UsersId",
                table: "NotificationUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationUser");

            migrationBuilder.DropColumn(
                name: "ImgURL",
                table: "Articles");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserNotifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                column: "ConcurrencyStamp",
                value: "8d4d3605-9c0a-4e64-aa64-facb0e79174b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                column: "ConcurrencyStamp",
                value: "6986b2ea-7722-418a-9eee-a5df79fd5f2c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                column: "ConcurrencyStamp",
                value: "a12e5c20-6352-41f0-b17e-fd29d833b410");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId",
                table: "UserNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
