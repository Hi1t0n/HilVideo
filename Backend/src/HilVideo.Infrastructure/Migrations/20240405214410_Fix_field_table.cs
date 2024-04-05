using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_field_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteMoviesUsers_Users_UserId1",
                table: "FavoriteMoviesUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesDirectors_Directors_DirectorId1",
                table: "MoviesDirectors");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesDirectors_Movies_MovieId1",
                table: "MoviesDirectors");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesGenres_Genres_GenreId1",
                table: "MoviesGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MoviesGenres_Movies_MovieId1",
                table: "MoviesGenres");

            migrationBuilder.DropIndex(
                name: "IX_MoviesGenres_GenreId1",
                table: "MoviesGenres");

            migrationBuilder.DropIndex(
                name: "IX_MoviesGenres_MovieId1",
                table: "MoviesGenres");

            migrationBuilder.DropIndex(
                name: "IX_MoviesDirectors_DirectorId1",
                table: "MoviesDirectors");

            migrationBuilder.DropIndex(
                name: "IX_MoviesDirectors_MovieId1",
                table: "MoviesDirectors");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteMoviesUsers_UserId1",
                table: "FavoriteMoviesUsers");

            migrationBuilder.DropColumn(
                name: "GenreId1",
                table: "MoviesGenres");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MoviesGenres");

            migrationBuilder.DropColumn(
                name: "DirectorId1",
                table: "MoviesDirectors");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MoviesDirectors");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "FavoriteMoviesUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GenreId1",
                table: "MoviesGenres",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId1",
                table: "MoviesGenres",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DirectorId1",
                table: "MoviesDirectors",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId1",
                table: "MoviesDirectors",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "FavoriteMoviesUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoviesGenres_GenreId1",
                table: "MoviesGenres",
                column: "GenreId1");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesGenres_MovieId1",
                table: "MoviesGenres",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesDirectors_DirectorId1",
                table: "MoviesDirectors",
                column: "DirectorId1");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesDirectors_MovieId1",
                table: "MoviesDirectors",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteMoviesUsers_UserId1",
                table: "FavoriteMoviesUsers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteMoviesUsers_Users_UserId1",
                table: "FavoriteMoviesUsers",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesDirectors_Directors_DirectorId1",
                table: "MoviesDirectors",
                column: "DirectorId1",
                principalTable: "Directors",
                principalColumn: "DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesDirectors_Movies_MovieId1",
                table: "MoviesDirectors",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesGenres_Genres_GenreId1",
                table: "MoviesGenres",
                column: "GenreId1",
                principalTable: "Genres",
                principalColumn: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoviesGenres_Movies_MovieId1",
                table: "MoviesGenres",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "MovieId");
        }
    }
}
