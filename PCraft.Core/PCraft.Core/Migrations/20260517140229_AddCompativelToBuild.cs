using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCraft.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddCompativelToBuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Compativel",
                table: "BuildsSalvas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Compativel",
                table: "BuildsSalvas");
        }
    }
}
