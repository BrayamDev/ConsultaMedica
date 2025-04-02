using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class Pacientes
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string? Nombre { get; set; }

        [StringLength(100)]
        public string? PrimerApellido { get; set; }

        [StringLength(100)]
        public string? SegundoApellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [StringLength(20)]
        public string? Sexo { get; set; }

        [StringLength(100)]
        public string? PaisOrigen { get; set; }

        [StringLength(100)]
        public string? Provincia { get; set; }

        [StringLength(100)]
        public string? Poblacion { get; set; }
    
        [Column("IdTipoDocumento")] // Mapea a la columna real en BD
        public int TipoDocumentoId { get; set; }

        [ForeignKey("TipoDocumentoId")]
        public TipoDocumento TipoDocumento { get; set; }

        [StringLength(50)]
        public string? NumeroDocumento { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        [StringLength(20)]
        public string? CodigoPostal { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(20)]
        public string? Movil { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Procedencia { get; set; }

        [StringLength(100)]
        public string? Aseguradora { get; set; }

        public string? Observaciones { get; set; }

        // Propiedades de navegación
        public ICollection<HistoriasClinicas> HistoriasClinicas { get; set; } = new List<HistoriasClinicas>();
        public ICollection<Citas> Citas { get; set; } = new List<Citas>();


    }
}
