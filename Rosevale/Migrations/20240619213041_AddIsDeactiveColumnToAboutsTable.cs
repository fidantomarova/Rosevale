using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rosevale.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeactiveColumnToAboutsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Abouts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Abouts");
        }
    }
}
