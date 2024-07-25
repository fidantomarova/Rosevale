using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rosevale.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeactiveColumnToStaffsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Staffs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Staffs");
        }
    }
}
