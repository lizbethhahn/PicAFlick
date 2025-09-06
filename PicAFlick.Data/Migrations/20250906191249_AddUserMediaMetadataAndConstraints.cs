using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicAFlick.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMediaMetadataAndConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserId_TmdbId",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "DateWatched",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Overview",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "PosterPath",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "UserRating",
                table: "UserMedia");

            migrationBuilder.DropColumn(
                name: "Watched",
                table: "UserMedia");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "UserMedia",
                newName: "PosterPath");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "WatchlistItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Overview",
                table: "UserMedia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserId_UserMediaId",
                table: "WatchlistItems",
                columns: new[] { "UserId", "UserMediaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMedia_TmdbId",
                table: "UserMedia",
                column: "TmdbId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserId_UserMediaId",
                table: "WatchlistItems");

            migrationBuilder.DropIndex(
                name: "IX_UserMedia_TmdbId",
                table: "UserMedia");

            migrationBuilder.DropColumn(
                name: "Overview",
                table: "UserMedia");

            migrationBuilder.RenameColumn(
                name: "PosterPath",
                table: "UserMedia",
                newName: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "WatchlistItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "WatchlistItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateWatched",
                table: "WatchlistItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "WatchlistItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Overview",
                table: "WatchlistItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterPath",
                table: "WatchlistItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Rating",
                table: "WatchlistItems",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "WatchlistItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "WatchlistItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "WatchlistItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UserRating",
                table: "UserMedia",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Watched",
                table: "UserMedia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserId_TmdbId",
                table: "WatchlistItems",
                columns: new[] { "UserId", "TmdbId" },
                unique: true);
        }
    }
}
