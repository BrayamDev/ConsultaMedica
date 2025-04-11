using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class ExamenFisico
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdHistoriaClinica { get; set; }

        [ForeignKey("IdHistoriaClinica")]
        public HistoriasClinicas HistoriaClinica { get; set; }

        public string Temperatura { get; set; }
        public string FrecuenciaCardiaca { get; set; }
        public string TensionArterial { get; set; }
        public string FrecuenciaRespiratoria { get; set; }
        public string SatO2 { get; set; }
        public string? ObservacionesExamenFisico { get; set; }
    }
}
