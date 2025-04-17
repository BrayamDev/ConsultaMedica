using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultaMedica.Migrations
{
    /// <inheritdoc />
    public partial class PropiedadesDeFacturaquitadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Tratamientos_TratamientoId",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_TratamientoId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "TratamientoId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Unidad",
                table: "Facturas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TratamientoId",
                table: "Facturas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Unidad",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_TratamientoId",
                table: "Facturas",
                column: "TratamientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Tratamientos_TratamientoId",
                table: "Facturas",
                column: "TratamientoId",
                principalTable: "Tratamientos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
