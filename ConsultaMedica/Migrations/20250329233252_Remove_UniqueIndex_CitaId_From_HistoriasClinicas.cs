using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultaMedica.Migrations
{
    /// <inheritdoc />
    public partial class Remove_UniqueIndex_CitaId_From_HistoriasClinicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_historiasClinicas_CitaId",
                table: "historiasClinicas");

            migrationBuilder.AlterColumn<int>(
                name: "CitaId",
                table: "historiasClinicas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_historiasClinicas_CitaId",
                table: "historiasClinicas",
                column: "CitaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_historiasClinicas_CitaId",
                table: "historiasClinicas");

            migrationBuilder.AlterColumn<int>(
                name: "CitaId",
                table: "historiasClinicas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_historiasClinicas_CitaId",
                table: "historiasClinicas",
                column: "CitaId",
                unique: true,
                filter: "[CitaId] IS NOT NULL");
        }
    }
}
