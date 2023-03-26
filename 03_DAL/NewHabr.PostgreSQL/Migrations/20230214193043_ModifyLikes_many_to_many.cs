using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewHabr.PostgreSQL.Migrations
{
    public partial class ModifyLikes_many_to_many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleUser_Articles_LikedArticlesId",
                table: "ArticleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleUser_AspNetUsers_LikesId",
                table: "ArticleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentUser_AspNetUsers_LikesId",
                table: "CommentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentUser_Comments_LikedCommentsId",
                table: "CommentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_AspNetUsers_LikedUsersId",
                table: "UserUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_AspNetUsers_ReceivedLikesId",
                table: "UserUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUser",
                table: "UserUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentUser",
                table: "CommentUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleUser",
                table: "ArticleUser");

            migrationBuilder.RenameTable(
                name: "UserUser",
                newName: "UserLikesAuthor");

            migrationBuilder.RenameTable(
                name: "CommentUser",
                newName: "UserLikesComment");

            migrationBuilder.RenameTable(
                name: "ArticleUser",
                newName: "UserLikesArticle");

            migrationBuilder.RenameIndex(
                name: "IX_UserUser_ReceivedLikesId",
                table: "UserLikesAuthor",
                newName: "IX_UserLikesAuthor_ReceivedLikesId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentUser_LikesId",
                table: "UserLikesComment",
                newName: "IX_UserLikesComment_LikesId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleUser_LikesId",
                table: "UserLikesArticle",
                newName: "IX_UserLikesArticle_LikesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikesAuthor",
                table: "UserLikesAuthor",
                columns: new[] { "LikedUsersId", "ReceivedLikesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikesComment",
                table: "UserLikesComment",
                columns: new[] { "LikedCommentsId", "LikesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikesArticle",
                table: "UserLikesArticle",
                columns: new[] { "LikedArticlesId", "LikesId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikesArticle_Articles_LikedArticlesId",
                table: "UserLikesArticle",
                column: "LikedArticlesId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikesArticle_AspNetUsers_LikesId",
                table: "UserLikesArticle",
                column: "LikesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikesAuthor_AspNetUsers_LikedUsersId",
                table: "UserLikesAuthor",
                column: "LikedUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikesAuthor_AspNetUsers_ReceivedLikesId",
                table: "UserLikesAuthor",
                column: "ReceivedLikesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikesComment_AspNetUsers_LikesId",
                table: "UserLikesComment",
                column: "LikesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikesComment_Comments_LikedCommentsId",
                table: "UserLikesComment",
                column: "LikedCommentsId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikesArticle_Articles_LikedArticlesId",
                table: "UserLikesArticle");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikesArticle_AspNetUsers_LikesId",
                table: "UserLikesArticle");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikesAuthor_AspNetUsers_LikedUsersId",
                table: "UserLikesAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikesAuthor_AspNetUsers_ReceivedLikesId",
                table: "UserLikesAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikesComment_AspNetUsers_LikesId",
                table: "UserLikesComment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikesComment_Comments_LikedCommentsId",
                table: "UserLikesComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikesComment",
                table: "UserLikesComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikesAuthor",
                table: "UserLikesAuthor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikesArticle",
                table: "UserLikesArticle");

            migrationBuilder.RenameTable(
                name: "UserLikesComment",
                newName: "CommentUser");

            migrationBuilder.RenameTable(
                name: "UserLikesAuthor",
                newName: "UserUser");

            migrationBuilder.RenameTable(
                name: "UserLikesArticle",
                newName: "ArticleUser");

            migrationBuilder.RenameIndex(
                name: "IX_UserLikesComment_LikesId",
                table: "CommentUser",
                newName: "IX_CommentUser_LikesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLikesAuthor_ReceivedLikesId",
                table: "UserUser",
                newName: "IX_UserUser_ReceivedLikesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLikesArticle_LikesId",
                table: "ArticleUser",
                newName: "IX_ArticleUser_LikesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentUser",
                table: "CommentUser",
                columns: new[] { "LikedCommentsId", "LikesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUser",
                table: "UserUser",
                columns: new[] { "LikedUsersId", "ReceivedLikesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleUser",
                table: "ArticleUser",
                columns: new[] { "LikedArticlesId", "LikesId" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("00a98c8e-6a15-4447-9343-063f4f1efefc"),
                column: "ConcurrencyStamp",
                value: "e8d8d78e-a1f8-4dc6-9b91-f032e9f9826c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1bfc496b-ebd2-4c5a-b3e8-4b2c1e334391"),
                column: "ConcurrencyStamp",
                value: "4b5aa11a-fffb-478a-bee8-056b0e74154b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aec1eede-5f3f-43ba-9ec3-454a3002c013"),
                column: "ConcurrencyStamp",
                value: "3112daf5-972f-4163-bcf2-4b081605bc61");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleUser_Articles_LikedArticlesId",
                table: "ArticleUser",
                column: "LikedArticlesId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleUser_AspNetUsers_LikesId",
                table: "ArticleUser",
                column: "LikesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentUser_AspNetUsers_LikesId",
                table: "CommentUser",
                column: "LikesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentUser_Comments_LikedCommentsId",
                table: "CommentUser",
                column: "LikedCommentsId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_AspNetUsers_LikedUsersId",
                table: "UserUser",
                column: "LikedUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_AspNetUsers_ReceivedLikesId",
                table: "UserUser",
                column: "ReceivedLikesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
