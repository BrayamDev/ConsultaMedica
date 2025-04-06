using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultaMedica.Migrations
{
    /// <inheritdoc />
    public partial class PropiedadEnfermedadBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnfermedadBase",
                table: "historiasClinicas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnfermedadBase",
                table: "historiasClinicas");
        }
    }
}
