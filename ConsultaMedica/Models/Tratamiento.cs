using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class Tratamiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(200)]
        public string NombreTratamiento { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ImporteUnitario { get; set; }

        [Required]
        public int EspecialidadId { get; set; }

        [ForeignKey("EspecialidadId")]
        public virtual Especialidades Especialidad { get; set; }
    }
}
