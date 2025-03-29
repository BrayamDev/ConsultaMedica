using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class ProcedimientoProfesional
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdHistoriaClinica { get; set; }

        [ForeignKey("IdHistoriaClinica")]
        public HistoriasClinicas HistoriaClinica { get; set; }

        [Required]
        public string NombreProcedimiento { get; set; }

        public string Observaciones { get; set; }

        public DateTime FechaProcedimiento { get; set; } = DateTime.Now;

        public string NombreProfesional { get; set; }
    }
}
