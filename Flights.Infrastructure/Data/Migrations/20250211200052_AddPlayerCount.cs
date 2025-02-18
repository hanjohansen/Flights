﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirstRoundPlayersPerGame",
                table: "tournaments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstRoundPlayersPerGame",
                table: "tournaments");
        }
    }
}
