using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApp.API.Migrations
{
    /// <inheritdoc />
    public partial class snakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genres_GenreId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_SubGenres_SubGenreId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_SubGenres_Genres_GenreId",
                table: "SubGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubGenres",
                table: "SubGenres");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "movies");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "genres");

            migrationBuilder.RenameTable(
                name: "SubGenres",
                newName: "sub_genres");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "users",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "users",
                newName: "password_salt");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "users",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "users",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "users",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "movies",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "movies",
                newName: "picture");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "movies",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Audience",
                table: "movies",
                newName: "audience");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "movies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SubGenreId",
                table: "movies",
                newName: "sub_genre_id");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "movies",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "movies",
                newName: "date_created");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_SubGenreId",
                table: "movies",
                newName: "IX_movies_sub_genre_id");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_GenreId",
                table: "movies",
                newName: "IX_movies_genre_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "genres",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "genres",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "genres",
                newName: "date_created");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "sub_genres",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sub_genres",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "sub_genres",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "sub_genres",
                newName: "date_created");

            migrationBuilder.RenameIndex(
                name: "IX_SubGenres_GenreId",
                table: "sub_genres",
                newName: "IX_sub_genres_genre_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movies",
                table: "movies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_genres",
                table: "genres",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sub_genres",
                table: "sub_genres",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_movies_genres_genre_id",
                table: "movies",
                column: "genre_id",
                principalTable: "genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movies_sub_genres_sub_genre_id",
                table: "movies",
                column: "sub_genre_id",
                principalTable: "sub_genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sub_genres_genres_genre_id",
                table: "sub_genres",
                column: "genre_id",
                principalTable: "genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movies_genres_genre_id",
                table: "movies");

            migrationBuilder.DropForeignKey(
                name: "FK_movies_sub_genres_sub_genre_id",
                table: "movies");

            migrationBuilder.DropForeignKey(
                name: "FK_sub_genres_genres_genre_id",
                table: "sub_genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movies",
                table: "movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_genres",
                table: "genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sub_genres",
                table: "sub_genres");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "movies",
                newName: "Movies");

            migrationBuilder.RenameTable(
                name: "genres",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "sub_genres",
                newName: "SubGenres");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "password_salt",
                table: "Users",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Movies",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "picture",
                table: "Movies",
                newName: "Picture");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Movies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "audience",
                table: "Movies",
                newName: "Audience");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Movies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "sub_genre_id",
                table: "Movies",
                newName: "SubGenreId");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                table: "Movies",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "date_created",
                table: "Movies",
                newName: "DateCreated");

            migrationBuilder.RenameIndex(
                name: "IX_movies_sub_genre_id",
                table: "Movies",
                newName: "IX_Movies_SubGenreId");

            migrationBuilder.RenameIndex(
                name: "IX_movies_genre_id",
                table: "Movies",
                newName: "IX_Movies_GenreId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Genres",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "date_created",
                table: "Genres",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "SubGenres",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SubGenres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                table: "SubGenres",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "date_created",
                table: "SubGenres",
                newName: "DateCreated");

            migrationBuilder.RenameIndex(
                name: "IX_sub_genres_genre_id",
                table: "SubGenres",
                newName: "IX_SubGenres_GenreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubGenres",
                table: "SubGenres",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genres_GenreId",
                table: "Movies",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_SubGenres_SubGenreId",
                table: "Movies",
                column: "SubGenreId",
                principalTable: "SubGenres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubGenres_Genres_GenreId",
                table: "SubGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
