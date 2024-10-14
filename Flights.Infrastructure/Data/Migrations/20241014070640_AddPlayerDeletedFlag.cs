using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerDeletedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_players_Name",
                table: "players");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "players");

            migrationBuilder.CreateIndex(
                name: "IX_players_Name",
                table: "players",
                column: "Name",
                unique: true);
        }
    }
}
