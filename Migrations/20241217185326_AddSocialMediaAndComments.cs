using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialMediaAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "comments",
                newName: "user_id");

            migrationBuilder.AddColumn<string>(
                name: "facebook_link",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "instagram_link",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "linked_in_link",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "profile_image_url",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "twitter_link",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_id1",
                table: "comments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id1",
                table: "comments",
                column: "user_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_user_id1",
                table: "comments",
                column: "user_id1",
                principalTable: "users",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_user_id1",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_user_id1",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "facebook_link",
                table: "users");

            migrationBuilder.DropColumn(
                name: "instagram_link",
                table: "users");

            migrationBuilder.DropColumn(
                name: "linked_in_link",
                table: "users");

            migrationBuilder.DropColumn(
                name: "profile_image_url",
                table: "users");

            migrationBuilder.DropColumn(
                name: "twitter_link",
                table: "users");

            migrationBuilder.DropColumn(
                name: "user_id1",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "comments",
                newName: "user_name");
        }
    }
}
