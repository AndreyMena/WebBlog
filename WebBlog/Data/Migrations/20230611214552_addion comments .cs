using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBlog.Data.Migrations
{
    public partial class addioncomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BlogPostId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_BlogPostId",
                table: "Comment",
                column: "BlogPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_BlogPost_BlogPostId",
                table: "Comment",
                column: "BlogPostId",
                principalTable: "BlogPost",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_BlogPost_BlogPostId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_BlogPostId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "BlogPostId",
                table: "Comment");
        }
    }
}
