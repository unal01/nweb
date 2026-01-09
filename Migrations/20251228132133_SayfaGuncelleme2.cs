using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBuilder.Migrations
{
    /// <inheritdoc />
    public partial class SayfaGuncelleme2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Pages");

            migrationBuilder.AddColumn<bool>(
                name: "HasAnnouncements",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasGallery",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasNews",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSlider",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PageType",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Pages",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAnnouncements",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "HasGallery",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "HasNews",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "HasSlider",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "PageType",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Pages");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Pages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
