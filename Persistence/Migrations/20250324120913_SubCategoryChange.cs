using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SubCategoryChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SubCategories",
                newName: "TitlePL");

            migrationBuilder.AddColumn<string>(
                name: "TitleEN",
                table: "SubCategories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleEN",
                table: "SubCategories");

            migrationBuilder.RenameColumn(
                name: "TitlePL",
                table: "SubCategories",
                newName: "Title");
        }
    }
}
