using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFinishBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FinishLocked",
                table: "games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            
            migrationBuilder.Sql("UPDATE games SET FinishLocked = true where (Finished IS NOT NULL) AND (TournamentGameId IS NOT NULL)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishLocked",
                table: "games");
        }
    }
}
