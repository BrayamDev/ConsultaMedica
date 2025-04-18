using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Número de Factura")]
        public string NumeroFactura { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Factura")]
        public DateTime FechaFactura { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Empresa")]
        public string Empresa { get; set; }

        [Required]
        [Display(Name = "Tipo de Cobro")]
        public string TipoCobro { get; set; } 

        [Display(Name = "Observaciones del Cobro")]
        [StringLength(500)]
        public string ObservacionesCobro { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Importe Total")]
        public decimal ImporteTotal { get; set; }
        public bool EsFactura { get; set; }

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public virtual Pacientes Paciente { get; set; }

        // Relación con Cita (opcional)
        public int? CitaId { get; set; }
        [ForeignKey("CitaId")]
        public virtual Citas Cita { get; set; }
        public ICollection<TratamientoFacturado> TratamientosFacturados { get; set; }


    }
}
