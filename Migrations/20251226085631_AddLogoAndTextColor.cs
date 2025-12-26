using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBuilder.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoAndTextColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "ThemeSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MenuTextColor",
                table: "ThemeSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "MenuTextColor",
                table: "ThemeSettings");
        }
    }
}
