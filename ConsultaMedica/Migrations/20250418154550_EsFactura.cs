using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultaMedica.Migrations
{
    /// <inheritdoc />
    public partial class EsFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsFactura",
                table: "Facturas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsFactura",
                table: "Facturas");
        }
    }
}
