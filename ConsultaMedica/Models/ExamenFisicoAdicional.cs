using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class ExamenFisicoAdicional
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdHistoriaClinica { get; set; }

        [ForeignKey("IdHistoriaClinica")]
        public HistoriasClinicas HistoriaClinica { get; set; }

        [Required]
        public string NombreItem { get; set; }

        public string Observaciones { get; set; }
    }
}
