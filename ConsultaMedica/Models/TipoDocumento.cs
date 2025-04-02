using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    // Agrega esta clase en tu carpeta Models
    public class TipoDocumento
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        // Relación inversa
        public ICollection<Pacientes> Pacientes { get; set; }
    }
}
