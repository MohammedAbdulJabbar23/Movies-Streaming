using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApp.API.Migrations
{
    /// <inheritdoc />
    public partial class changetoforignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_user_model_user_id",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_user_model_user_id",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "user_model_user_id",
                table: "comments");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_user_id",
                table: "comments",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_user_id",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_user_id",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "comments");

            migrationBuilder.AddColumn<int>(
                name: "user_model_user_id",
                table: "comments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_model_user_id",
                table: "comments",
                column: "user_model_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_user_model_user_id",
                table: "comments",
                column: "user_model_user_id",
                principalTable: "users",
                principalColumn: "user_id");
        }
    }
}
