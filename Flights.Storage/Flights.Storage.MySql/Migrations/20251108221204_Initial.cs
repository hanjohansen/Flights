using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Storage.MySql
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TournamentNumber = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstRoundPlayersPerGame = table.Column<int>(type: "int", nullable: false),
                    SemiFinalWithLosersCup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    X01Target = table.Column<int>(type: "int", nullable: false),
                    InModifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OutModifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Started = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Finished = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournaments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "player_files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StoragePath = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_player_files_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tournament_players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tournament_rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WildCardId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tournament_games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TournamentRoundId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    IsLosersCup = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameNumber = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    X01Target = table.Column<int>(type: "int", nullable: false),
                    InModifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OutModifier = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FinishAfterFirstRank = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Started = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Finished = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    FinishLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TournamentGameId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_games_tournament_games_TournamentGameId",
                        column: x => x.TournamentGameId,
                        principalTable: "tournament_games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "game_players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_game_players_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_players_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "game_rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GameId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_game_rounds_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "round_stat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoundId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    IsIn = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsBust = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: true),
                    StartPoints = table.Column<int>(type: "int", nullable: false),
                    EndPoints = table.Column<int>(type: "int", nullable: false),
                    FirstDart_Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstDart_Value = table.Column<int>(type: "int", nullable: true),
                    SecondDart_Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecondDart_Value = table.Column<int>(type: "int", nullable: true),
                    ThirdDart_Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThirdDart_Value = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_round_stat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_round_stat_game_rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "game_rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_round_stat_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_game_players_GameId",
                table: "game_players",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_game_players_PlayerId",
                table: "game_players",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_game_rounds_GameId",
                table: "game_rounds",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_games_TournamentGameId",
                table: "games",
                column: "TournamentGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_player_files_PlayerId",
                table: "player_files",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_round_stat_PlayerId",
                table: "round_stat",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_round_stat_RoundId",
                table: "round_stat",
                column: "RoundId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_players");

            migrationBuilder.DropTable(
                name: "player_files");

            migrationBuilder.DropTable(
                name: "round_stat");

            migrationBuilder.DropTable(
                name: "game_rounds");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "tournament_games");

            migrationBuilder.DropTable(
                name: "tournament_rounds");

            migrationBuilder.DropTable(
                name: "tournament_players");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "tournaments");
        }
    }
}
