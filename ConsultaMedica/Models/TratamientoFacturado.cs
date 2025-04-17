using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class TratamientoFacturado
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public int TratamientoId { get; set; }

        [Required]
        [Display(Name = "Nombre del Tratamiento")]
        public string NombreTratamiento { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Las unidades deben ser al menos 1")]
        public int Unidades { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor que 0")]
        [Display(Name = "Precio Unitario")]
        public decimal ImporteUnitario { get; set; }

        [Display(Name = "Observaciones")]
        public string ObservacionesTratamiento { get; set; }

        [Required]
        public decimal Total { get; set; }

        // Relaciones
        public Factura Factura { get; set; }
        public Tratamiento Tratamiento { get; set; }

    }
}
