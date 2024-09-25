using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    X01Target = table.Column<int>(type: "INTEGER", nullable: false),
                    InModifier = table.Column<string>(type: "TEXT", nullable: false),
                    OutModifier = table.Column<string>(type: "TEXT", nullable: false),
                    Started = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Finished = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "game_rounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "game_players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderNumber = table.Column<int>(type: "INTEGER", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "round_stat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoundId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    IsIn = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBust = table.Column<bool>(type: "INTEGER", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: true),
                    StartPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    EndPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstDart_Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    FirstDart_Value = table.Column<int>(type: "INTEGER", nullable: true),
                    SecondDart_Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    SecondDart_Value = table.Column<int>(type: "INTEGER", nullable: true),
                    ThirdDart_Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    ThirdDart_Value = table.Column<int>(type: "INTEGER", nullable: true)
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
                });

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
                name: "IX_players_Name",
                table: "players",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_round_stat_PlayerId",
                table: "round_stat",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_round_stat_RoundId",
                table: "round_stat",
                column: "RoundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_players");

            migrationBuilder.DropTable(
                name: "round_stat");

            migrationBuilder.DropTable(
                name: "game_rounds");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "games");
        }
    }
}
