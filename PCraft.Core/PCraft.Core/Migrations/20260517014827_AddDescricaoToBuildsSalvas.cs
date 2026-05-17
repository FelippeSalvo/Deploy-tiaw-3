using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCraft.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddDescricaoToBuildsSalvas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "BuildsSalvas",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "BuildsSalvas");
        }
    }
}
