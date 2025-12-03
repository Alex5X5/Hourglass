using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hourglass.Database.Migrations
{
    /// <inheritdoc />
    public partial class _14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<char>(
                name: "blocksTime",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: '\0');
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "blocksTime",
                table: "Tasks");
        }
    }
}
