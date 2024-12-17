using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApp.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeItToInteger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_user_id1",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "user_id1",
                table: "comments",
                newName: "user_model_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_comments_user_id1",
                table: "comments",
                newName: "IX_comments_user_model_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_user_model_user_id",
                table: "comments",
                column: "user_model_user_id",
                principalTable: "users",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_user_model_user_id",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "user_model_user_id",
                table: "comments",
                newName: "user_id1");

            migrationBuilder.RenameIndex(
                name: "IX_comments_user_model_user_id",
                table: "comments",
                newName: "IX_comments_user_id1");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "comments",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_user_id1",
                table: "comments",
                column: "user_id1",
                principalTable: "users",
                principalColumn: "user_id");
        }
    }
}
