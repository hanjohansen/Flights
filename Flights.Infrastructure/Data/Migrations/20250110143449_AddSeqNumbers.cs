﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeqNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentNumber",
                table: "tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameNumber",
                table: "games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("update games set GameNumber = rowid where TournamentGameId IS NULL");
            migrationBuilder.Sql("update tournaments set TournamentNumber = rowid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TournamentNumber",
                table: "tournaments");

            migrationBuilder.DropColumn(
                name: "GameNumber",
                table: "games");
        }
    }
}
