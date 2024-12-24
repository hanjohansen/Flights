using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TournamentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TournamentGameId",
                table: "games",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    SemiFinalWithLosersCup = table.Column<bool>(type: "INTEGER", nullable: false),
                    X01Target = table.Column<int>(type: "INTEGER", nullable: false),
                    InModifier = table.Column<string>(type: "TEXT", nullable: false),
                    OutModifier = table.Column<string>(type: "TEXT", nullable: false),
                    Started = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Finished = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tournament_players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournament_players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tournament_players_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tournament_players_tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tournament_rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WildCardId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournament_rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tournament_rounds_tournament_players_WildCardId",
                        column: x => x.WildCardId,
                        principalTable: "tournament_players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tournament_rounds_tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tournament_games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentRoundId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    IsLosersCup = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournament_games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tournament_games_tournament_rounds_TournamentRoundId",
                        column: x => x.TournamentRoundId,
                        principalTable: "tournament_rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_games_TournamentGameId",
                table: "games",
                column: "TournamentGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tournament_games_TournamentRoundId",
                table: "tournament_games",
                column: "TournamentRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_tournament_players_PlayerId",
                table: "tournament_players",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_tournament_players_TournamentId",
                table: "tournament_players",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_tournament_rounds_TournamentId",
                table: "tournament_rounds",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_tournament_rounds_WildCardId",
                table: "tournament_rounds",
                column: "WildCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_games_tournament_games_TournamentGameId",
                table: "games",
                column: "TournamentGameId",
                principalTable: "tournament_games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_tournament_games_TournamentGameId",
                table: "games");

            migrationBuilder.DropTable(
                name: "tournament_games");

            migrationBuilder.DropTable(
                name: "tournament_rounds");

            migrationBuilder.DropTable(
                name: "tournament_players");

            migrationBuilder.DropTable(
                name: "tournaments");

            migrationBuilder.DropIndex(
                name: "IX_games_TournamentGameId",
                table: "games");

            migrationBuilder.DropColumn(
                name: "TournamentGameId",
                table: "games");
        }
    }
}
