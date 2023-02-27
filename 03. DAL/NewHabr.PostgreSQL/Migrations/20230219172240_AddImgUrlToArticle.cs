using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewHabr.PostgreSQL.Migrations
{
    public partial class AddImgUrlToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgURL",
                table: "Articles",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                column: "ConcurrencyStamp",
                value: "aada65cc-b576-4f4a-9f9d-ebfd0b9280bc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                column: "ConcurrencyStamp",
                value: "9fb110ea-6629-4101-afab-d66d6391efd1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                column: "ConcurrencyStamp",
                value: "933268c1-3283-414d-bd9a-6cebd1ece76a");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgURL",
                table: "Articles");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                column: "ConcurrencyStamp",
                value: "2863b9b8-f85f-4059-bb45-4f3cdf406bc4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                column: "ConcurrencyStamp",
                value: "b17c68ab-f2cc-4ff5-bd1e-3eab2c878943");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                column: "ConcurrencyStamp",
                value: "6dd93045-d806-477b-a63f-4f87d2a94a23");
        }
    }
}
