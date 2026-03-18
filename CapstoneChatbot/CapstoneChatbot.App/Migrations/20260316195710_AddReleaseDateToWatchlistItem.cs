using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneChatbot.App.Migrations
{
    /// <inheritdoc />
    public partial class AddReleaseDateToWatchlistItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "WatchlistItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "WatchlistItems");
        }
    }
}
