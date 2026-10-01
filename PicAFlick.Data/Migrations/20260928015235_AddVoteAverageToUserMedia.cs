using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicAFlick.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVoteAverageToUserMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "VoteAverage",
                table: "UserMedia",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteAverage",
                table: "UserMedia");
        }
    }
}
