using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewHabr.PostgreSQL.Migrations
{
    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedArticles_Users_UserId",
                table: "LikedArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedComments_Users_UserId",
                table: "LikedComments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedUsers_Users_UserId",
                table: "LikedUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedArticles_Users_UserId",
                table: "LikedArticles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedComments_Users_UserId",
                table: "LikedComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikedUsers_Users_UserId",
                table: "LikedUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedArticles_Users_UserId",
                table: "LikedArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedComments_Users_UserId",
                table: "LikedComments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikedUsers_Users_UserId",
                table: "LikedUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedArticles_Users_UserId",
                table: "LikedArticles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedComments_Users_UserId",
                table: "LikedComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LikedUsers_Users_UserId",
                table: "LikedUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
