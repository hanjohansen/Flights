using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Storage.MySql
{
    /// <inheritdoc />
    public partial class AddTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "tournaments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "players",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "games",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenants", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.AddForeignKey(
                name: "FK_games_tenants_TenantId",
                table: "games",
                column: "TenantId",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_players_tenants_TenantId",
                table: "players",
                column: "TenantId",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tournaments_tenants_TenantId",
                table: "tournaments",
                column: "TenantId",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
