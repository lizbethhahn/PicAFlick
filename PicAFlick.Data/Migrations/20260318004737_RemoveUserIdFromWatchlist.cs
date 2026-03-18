using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicAFlick.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdFromWatchlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserId_UserMediaId",
                table: "WatchlistItems");

            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserMediaId",
                table: "WatchlistItems");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WatchlistItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserMediaId",
                table: "WatchlistItems",
                column: "UserMediaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserMediaId",
                table: "WatchlistItems");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WatchlistItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserId_UserMediaId",
                table: "WatchlistItems",
                columns: new[] { "UserId", "UserMediaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserMediaId",
                table: "WatchlistItems",
                column: "UserMediaId");
        }
    }
}
