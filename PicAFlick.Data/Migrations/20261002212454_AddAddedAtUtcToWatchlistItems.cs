using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicAFlick.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAddedAtUtcToWatchlistItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAtUtc",
                table: "WatchlistItems",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedAtUtc",
                table: "WatchlistItems");
        }
    }
}
