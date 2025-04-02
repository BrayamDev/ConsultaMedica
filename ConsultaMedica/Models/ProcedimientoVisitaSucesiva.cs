using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ConsultaMedica.Models
{
    public class ProcedimientoVisitaSucesiva
    {
        public int Id { get; set; }

        [Required]
        public int IdVisitaSucesiva { get; set; }

        [Required]
        public DateTime FechaProcedimiento { get; set; } = DateTime.Now;

        [Required]
        public string Observaciones { get; set; }

        [Required]
        public int IdProfesional { get; set; }

        // Relaciones
        public VisitaSucesiva VisitaSucesiva { get; set; }
        public Doctores Profesional { get; set; }
    }
}
