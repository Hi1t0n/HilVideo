﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UserService.Infrastructure.Context;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240525171526_Add_All_Books_Table")]
    partial class Add_All_Books_Table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("UserService.Domain.Models.Author", b =>
                {
                    b.Property<Guid>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AuthorId");

                    b.HasIndex("AuthorId")
                        .IsUnique();

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("UserService.Domain.Models.Book", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BookDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BookFilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BookName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PosterFilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("BookId");

                    b.HasIndex("BookId")
                        .IsUnique();

                    b.ToTable("Books");
                });

            modelBuilder.Entity("UserService.Domain.Models.BookAuthor", b =>
                {
                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.HasKey("BookId", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.ToTable("BookAuthors");
                });

            modelBuilder.Entity("UserService.Domain.Models.BookGenre", b =>
                {
                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uuid");

                    b.HasKey("BookId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("BookGenres");
                });

            modelBuilder.Entity("UserService.Domain.Models.Director", b =>
                {
                    b.Property<Guid>("DirectorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DirectorId");

                    b.HasIndex("DirectorId")
                        .IsUnique();

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("UserService.Domain.Models.FavoriteBooksUsers", b =>
                {
                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("BookId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("FavoriteBooksUsers");
                });

            modelBuilder.Entity("UserService.Domain.Models.FavoriteMoviesUsers", b =>
                {
                    b.Property<Guid>("MovieId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("MovieId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("FavoriteMoviesUsers");
                });

            modelBuilder.Entity("UserService.Domain.Models.Genre", b =>
                {
                    b.Property<Guid>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("GenreName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("GenreId");

                    b.HasIndex("GenreId")
                        .IsUnique();

                    b.HasIndex("GenreName")
                        .IsUnique();

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("UserService.Domain.Models.Movie", b =>
                {
                    b.Property<Guid>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("MovieDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MovieName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("MovieTypeId")
                        .HasColumnType("uuid");

                    b.Property<string>("PosterFilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("MovieId");

                    b.HasIndex("MovieId")
                        .IsUnique();

                    b.HasIndex("MovieTypeId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("UserService.Domain.Models.MovieDirector", b =>
                {
                    b.Property<Guid>("MovieId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DirectorId")
                        .HasColumnType("uuid");

                    b.HasKey("MovieId", "DirectorId");

                    b.HasIndex("DirectorId");

                    b.ToTable("MoviesDirectors");
                });

            modelBuilder.Entity("UserService.Domain.Models.MovieFile", b =>
                {
                    b.Property<Guid>("MovieFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("EpisodNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uuid");

                    b.HasKey("MovieFileId");

                    b.HasIndex("FilePath")
                        .IsUnique();

                    b.HasIndex("MovieFileId")
                        .IsUnique();

                    b.HasIndex("MovieId");

                    b.ToTable("MovieFiles");
                });

            modelBuilder.Entity("UserService.Domain.Models.MovieGenre", b =>
                {
                    b.Property<Guid>("MovieId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uuid");

                    b.HasKey("MovieId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("MoviesGenres");
                });

            modelBuilder.Entity("UserService.Domain.Models.MovieType", b =>
                {
                    b.Property<Guid>("MovieTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("MovieTypeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("MovieTypeId");

                    b.HasIndex("MovieTypeId")
                        .IsUnique();

                    b.HasIndex("MovieTypeName")
                        .IsUnique();

                    b.ToTable("MovieTypes");
                });

            modelBuilder.Entity("UserService.Domain.Models.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("64141029-42fc-41f7-88eb-da0801efa3f3"),
                            RoleName = "User"
                        },
                        new
                        {
                            RoleId = new Guid("b2538ada-cd21-4e9c-9309-4b4062285da3"),
                            RoleName = "Admin"
                        },
                        new
                        {
                            RoleId = new Guid("f3def9dd-1bbe-47a0-80c1-d1bc0ab5a1f4"),
                            RoleName = "Owner"
                        });
                });

            modelBuilder.Entity("UserService.Domain.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("64141029-42fc-41f7-88eb-da0801efa3f3"));

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserService.Domain.Models.BookAuthor", b =>
                {
                    b.HasOne("UserService.Domain.Models.Author", "Author")
                        .WithMany("BookAuthors")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Domain.Models.Book", "Book")
                        .WithMany("BookAuthors")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("UserService.Domain.Models.BookGenre", b =>
                {
                    b.HasOne("UserService.Domain.Models.Book", "Book")
                        .WithMany("BookGenres")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Domain.Models.Genre", "Genre")
                        .WithMany("BookGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("UserService.Domain.Models.FavoriteBooksUsers", b =>
                {
                    b.HasOne("UserService.Domain.Models.Book", "Book")
                        .WithMany("FavoriteBooksUsers")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Domain.Models.User", "User")
                        .WithMany("FavoriteBooksUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Domain.Models.FavoriteMoviesUsers", b =>
                {
                    b.HasOne("UserService.Domain.Models.Movie", "Movie")
                        .WithMany("FavoriteMoviesUsers")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Domain.Models.User", "User")
                        .WithMany("FavoriteMoviesUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserService.Domain.Models.Movie", b =>
                {
                    b.HasOne("UserService.Domain.Models.MovieType", "MovieType")
                        .WithMany("Movies")
                        .HasForeignKey("MovieTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MovieType");
                });

            modelBuilder.Entity("UserService.Domain.Models.MovieDirector", b =>
                {
                    b.HasOne("UserService.Domain.Models.Director", "Director")
                        .WithMany("MovieDirectors")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Domain.Models.Movie", "Movie")
                        .WithMany("MovieDirectors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Director");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("UserService.Domain.Models.MovieFile", b =>
                {
                    b.HasOne("UserService.Domain.Models.Movie", "Movie")
                        .WithMany("MovieFiles")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("UserService.Domain.Models.MovieGenre", b =>
                {
                    b.HasOne("UserService.Domain.Models.Genre", "Genre")
                        .WithMany("MovieGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserService.Domain.Models.Movie", "Movie")
                        .WithMany("MovieGenres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("UserService.Domain.Models.User", b =>
                {
                    b.HasOne("UserService.Domain.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("UserService.Domain.Models.Author", b =>
                {
                    b.Navigation("BookAuthors");
                });

            modelBuilder.Entity("UserService.Domain.Models.Book", b =>
                {
                    b.Navigation("BookAuthors");

                    b.Navigation("BookGenres");

                    b.Navigation("FavoriteBooksUsers");
                });

            modelBuilder.Entity("UserService.Domain.Models.Director", b =>
                {
                    b.Navigation("MovieDirectors");
                });

            modelBuilder.Entity("UserService.Domain.Models.Genre", b =>
                {
                    b.Navigation("BookGenres");

                    b.Navigation("MovieGenres");
                });

            modelBuilder.Entity("UserService.Domain.Models.Movie", b =>
                {
                    b.Navigation("FavoriteMoviesUsers");

                    b.Navigation("MovieDirectors");

                    b.Navigation("MovieFiles");

                    b.Navigation("MovieGenres");
                });

            modelBuilder.Entity("UserService.Domain.Models.MovieType", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("UserService.Domain.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("UserService.Domain.Models.User", b =>
                {
                    b.Navigation("FavoriteBooksUsers");

                    b.Navigation("FavoriteMoviesUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
