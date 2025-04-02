using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ConsultaMedica.Models
{
    public class VisitaSucesiva
    {
        public int Id { get; set; }

        [Required]
        public int IdHistoriaClinica { get; set; }

        [Required]
        public int IdCita { get; set; }

        [Required]
        public DateTime FechaVisita { get; set; } = DateTime.Now;

        [Required]
        public string EvolucionAnalisis { get; set; }

        [Required]
        public string ConductaMedica { get; set; }

        [Required]
        public int IdMedicoResponsable { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relaciones
        public HistoriasClinicas HistoriaClinica { get; set; }
        public Citas Cita { get; set; }
        public Doctores MedicoResponsable { get; set; }
        public ICollection<ProcedimientoVisitaSucesiva> Procedimientos { get; set; }
    }
}
