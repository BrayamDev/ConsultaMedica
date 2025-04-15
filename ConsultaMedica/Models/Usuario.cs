using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class Usuario 
    {
        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(20)]
        public string? NumeroTelefono { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

    }
}
