using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rosevale.Migrations
{
    /// <inheritdoc />
    public partial class CreateAboutsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img1",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Img2",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Img3",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Img4",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Img1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Img2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Img3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Img4 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");

            migrationBuilder.DropColumn(
                name: "Img1",
                table: "Bios");

            migrationBuilder.DropColumn(
                name: "Img2",
                table: "Bios");

            migrationBuilder.DropColumn(
                name: "Img3",
                table: "Bios");

            migrationBuilder.DropColumn(
                name: "Img4",
                table: "Bios");
        }
    }
}
