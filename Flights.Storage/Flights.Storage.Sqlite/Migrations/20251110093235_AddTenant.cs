using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Storage.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AddTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var defaultTenantId = Guid.NewGuid().ToString().ToUpper();
            var defaultTenantName = "Garage";
            var defaultTenantPw = "$2a$13$zLFuXVs/6JlijULKdOPKlOfxjxct.HOExY6T3AZf.BF0L86x1s53K"; //'garage'

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenants", x => x.Id);
                });

            migrationBuilder.Sql($"INSERT INTO tenants (Id, Name, Password) VALUES ('{defaultTenantId}', '{defaultTenantName}', '{defaultTenantPw}')");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "tournaments",
                type: "TEXT",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "players",
                type: "TEXT",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "games",
                type: "TEXT",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_tournaments_TenantId",
                table: "tournaments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_players_TenantId",
                table: "players",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_games_TenantId",
                table: "games",
                column: "TenantId");

            migrationBuilder.Sql($"UPDATE tournaments SET TenantId = '{defaultTenantId}'");
            migrationBuilder.Sql($"UPDATE games SET TenantId = '{defaultTenantId}'");
            migrationBuilder.Sql($"UPDATE players SET TenantId = '{defaultTenantId}'");

            // //rebuild players
            migrationBuilder.Sql("CREATE TABLE \"tmp_players\" ("
                                    + "\"Id\" TEXT NOT NULL CONSTRAINT \"PK_players\" PRIMARY KEY,"
                                    + "\"TenantId\" TEXT NOT NULL,"
                                    + "\"Name\" TEXT NOT NULL,"
                                    + "\"Deleted\" INTEGER NOT NULL DEFAULT 0,"
                                    + "CONSTRAINT \"FK_players_tenant_players_TenantId\" FOREIGN KEY (\"TenantId\") REFERENCES \"tenants\" (\"Id\") ON DELETE CASCADE"
                                    + ");");

            migrationBuilder.Sql("insert into tmp_players (Id, TenantId, Name, Deleted) select Id, TenantId, Name, Deleted from players");
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;", true);
            migrationBuilder.Sql("drop table players", true);
            migrationBuilder.Sql("alter table tmp_players rename to players", true);
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;", true);
            migrationBuilder.Sql("CREATE INDEX \"IX_players_TenantId\" ON \"players\" (\"TenantId\")");

            //rebuild games
            migrationBuilder.Sql("CREATE TABLE \"tmp_games\" ("
                                    + "\"Id\" TEXT NOT NULL CONSTRAINT \"PK_games\" PRIMARY KEY,"
                                    + "\"TenantId\" TEXT NOT NULL,"
                                    + "\"FinishAfterFirstRank\" INTEGER NOT NULL,"
                                    + "\"Finished\" TEXT NULL,"
                                    + "\"InModifier\" TEXT NOT NULL,"
                                    + "\"OutModifier\" TEXT NOT NULL,"
                                    + "\"Started\" TEXT NOT NULL,"
                                    + "\"TournamentGameId\" TEXT NULL,"
                                    + "\"Type\" TEXT NOT NULL,"
                                    + "\"X01Target\" INTEGER NOT NULL, "
                                    + "\"GameNumber\" INTEGER NOT NULL DEFAULT 0, "
                                    + "\"FinishLocked\" INTEGER NOT NULL DEFAULT 0, "
                                    + "CONSTRAINT \"FK_games_tournament_games_TournamentGameId\" FOREIGN KEY (\"TournamentGameId\") REFERENCES \"tournament_games\" (\"Id\") ON DELETE CASCADE, "
                                    + "CONSTRAINT \"FK_games_tenants_TenantId\" FOREIGN KEY (\"TenantId\") REFERENCES \"tenants\" (\"Id\") ON DELETE CASCADE"
                                    + ");");

            migrationBuilder.Sql("INSERT INTO tmp_games (Id, TenantId, FinishAfterFirstRank, Finished, InModifier, OutModifier, Started, TournamentGameId, Type, X01Target, GameNumber, FinishLocked) " 
                                    + "SELECT Id, TenantId, FinishAfterFirstRank, Finished, InModifier, OutModifier, Started, TournamentGameId, Type, X01Target, GameNumber, FinishLocked from games");
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;", true);
            migrationBuilder.Sql("drop table games", true);
            migrationBuilder.Sql("alter table tmp_games rename to games", true);
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;", true);
            migrationBuilder.Sql("CREATE INDEX \"IX_games_TenantId\" ON \"games\" (\"TenantId\")");
            migrationBuilder.Sql("CREATE UNIQUE INDEX \"IX_games_TournamentGameId\" ON \"games\" (\"TournamentGameId\")");

            //rebuild tournaments
            migrationBuilder.Sql("CREATE TABLE \"tmp_tournaments\" ("
                                            + "\"Id\" TEXT NOT NULL CONSTRAINT \"PK_tournaments\" PRIMARY KEY, "
                                            + "\"TenantId\" TEXT NOT NULL,"
                                            + "\"Type\" TEXT NOT NULL, "
                                            + "\"SemiFinalWithLosersCup\" INTEGER NOT NULL, "
                                            + "\"X01Target\" INTEGER NOT NULL, "
                                            + "\"InModifier\" TEXT NOT NULL, "
                                            + "\"OutModifier\" TEXT NOT NULL, "
                                            + "\"Started\" TEXT NOT NULL, "
                                            + "\"Finished\" TEXT NULL, "
                                            + "\"TournamentNumber\" INTEGER NOT NULL DEFAULT 0, "
                                            + "\"FirstRoundPlayersPerGame\" INTEGER NOT NULL DEFAULT 2, "
                                            + "CONSTRAINT \"FK_tournaments_tenants_TenantId\" FOREIGN KEY (\"TenantId\") REFERENCES \"tenants\" (\"Id\") ON DELETE CASCADE"
                                            + ");");
            
            migrationBuilder.Sql("INSERT INTO tmp_tournaments (Id, TenantId, Type, SemiFinalWithLosersCup, X01Target, InModifier, OutModifier, Started, Finished, TournamentNumber, FirstRoundPlayersPerGame) " 
                                    + "SELECT Id, TenantId, Type, SemiFinalWithLosersCup, X01Target, InModifier, OutModifier, Started, Finished, TournamentNumber, FirstRoundPlayersPerGame from tournaments");
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;", true);
            migrationBuilder.Sql("drop table tournaments", true);
            migrationBuilder.Sql("alter table tmp_tournaments rename to tournaments", true);
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;", true);
            migrationBuilder.Sql("CREATE INDEX \"IX_tournaments_TenantId\" ON \"tournaments\" (\"TenantId\")");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_tenants_TenantId",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "FK_players_tenants_TenantId",
                table: "players");

            migrationBuilder.DropForeignKey(
                name: "FK_tournaments_tenants_TenantId",
                table: "tournaments");

            migrationBuilder.DropTable(
                name: "tenants");

            migrationBuilder.DropIndex(
                name: "IX_tournaments_TenantId",
                table: "tournaments");

            migrationBuilder.DropIndex(
                name: "IX_players_TenantId",
                table: "players");

            migrationBuilder.DropIndex(
                name: "IX_games_TenantId",
                table: "games");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "tournaments");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "players");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "games");
        }
    }
}
