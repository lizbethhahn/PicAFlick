using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicAFlick.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWatchlistItemUserMediaFK_And_UpdateUserRatingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserMedia_Id",
                table: "UserMedia");

            migrationBuilder.AddColumn<int>(
                name: "UserMediaId",
                table: "WatchlistItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "UserRating",
                table: "UserMedia",
                type: "decimal(3,1)",
                precision: 3,
                scale: 1,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldPrecision: 3,
                oldScale: 1,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistItems_UserMediaId",
                table: "WatchlistItems",
                column: "UserMediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchlistItems_UserMedia_UserMediaId",
                table: "WatchlistItems",
                column: "UserMediaId",
                principalTable: "UserMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchlistItems_UserMedia_UserMediaId",
                table: "WatchlistItems");

            migrationBuilder.DropIndex(
                name: "IX_WatchlistItems_UserMediaId",
                table: "WatchlistItems");

            migrationBuilder.DropColumn(
                name: "UserMediaId",
                table: "WatchlistItems");

            migrationBuilder.AlterColumn<int>(
                name: "UserRating",
                table: "UserMedia",
                type: "int",
                precision: 3,
                scale: 1,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldPrecision: 3,
                oldScale: 1,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMedia_Id",
                table: "UserMedia",
                column: "Id",
                unique: true);
        }
    }
}
