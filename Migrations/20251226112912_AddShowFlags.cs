using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBuilder.Migrations
{
    /// <inheritdoc />
    public partial class AddShowFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowLogo",
                table: "ThemeSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowSiteName",
                table: "ThemeSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowLogo",
                table: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "ShowSiteName",
                table: "ThemeSettings");
        }
    }
}
