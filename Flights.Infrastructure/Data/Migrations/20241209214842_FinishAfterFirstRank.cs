using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinishAfterFirstRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FinishAfterFirstRank",
                table: "games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishAfterFirstRank",
                table: "games");
        }
    }
}
