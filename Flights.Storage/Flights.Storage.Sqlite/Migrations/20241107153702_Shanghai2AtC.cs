using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flights.Storage.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Shanghai2AtC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update games set type = 'AroundTheClock' where type = 'Shanghai'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update games set type = 'Shanghai' where type = 'AroundTheClock'");
        }
    }
}
