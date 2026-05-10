using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PCraft.Core.Data;

#nullable disable

namespace PCraft.Core.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20260210120000_SistemaBuildsSalvas")]
    public class SistemaBuildsSalvas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuildsSalvas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Compartilhada = table.Column<bool>(type: "boolean", nullable: false),
                    CriadaEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    CpuId = table.Column<int>(type: "integer", nullable: true),
                    MotherboardId = table.Column<int>(type: "integer", nullable: true),
                    RamId = table.Column<int>(type: "integer", nullable: true),
                    GpuId = table.Column<int>(type: "integer", nullable: true),
                    PsuId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildsSalvas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildsSalvas_CPUs_CpuId",
                        column: x => x.CpuId,
                        principalTable: "CPUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BuildsSalvas_GPUs_GpuId",
                        column: x => x.GpuId,
                        principalTable: "GPUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BuildsSalvas_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BuildsSalvas_PSUs_PsuId",
                        column: x => x.PsuId,
                        principalTable: "PSUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BuildsSalvas_RAMs_RamId",
                        column: x => x.RamId,
                        principalTable: "RAMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BuildsSalvas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuildsSalvas_CpuId",
                table: "BuildsSalvas",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildsSalvas_GpuId",
                table: "BuildsSalvas",
                column: "GpuId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildsSalvas_MotherboardId",
                table: "BuildsSalvas",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildsSalvas_PsuId",
                table: "BuildsSalvas",
                column: "PsuId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildsSalvas_RamId",
                table: "BuildsSalvas",
                column: "RamId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildsSalvas_UsuarioId",
                table: "BuildsSalvas",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildsSalvas");
        }
    }
}
