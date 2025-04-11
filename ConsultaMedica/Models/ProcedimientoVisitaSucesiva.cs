using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ConsultaMedica.Models
{
    public class ProcedimientoVisitaSucesiva
    {
        public int Id { get; set; }
        public int IdVisitaSucesiva { get; set; }
        public DateTime FechaProcedimiento { get; set; }
        public string Observaciones { get; set; }

        // Solo mantener esta
        public int ProfesionalId { get; set; }

        // Propiedades de navegación
        public virtual VisitaSucesiva VisitaSucesiva { get; set; }
        public virtual Doctores Profesional { get; set; }
    }
}
