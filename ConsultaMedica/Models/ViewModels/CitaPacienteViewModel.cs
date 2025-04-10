using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models.ViewModels
{
    public class CitaPacienteViewModel
    {

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Sexo")]
        public string Sexo { get; set; } = "Masculino";

        [Display(Name = "País de Origen")]
        public string PaisOrigen { get; set; }

        [Display(Name = "Provincia")]
        public string Provincia { get; set; }

        [Display(Name = "Población")]
        public string Poblacion { get; set; }

        [Display(Name = "Tipo de Documento")]
        public int? TipoDocumentoId { get; set; }

        [Display(Name = "Número de Documento")]
        public string NumeroDocumento { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Phone(ErrorMessage = "El móvil no tiene un formato válido")]
        [Display(Name = "Móvil")]
        public string Movil { get; set; }

        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Procedencia")]
        public string Procedencia { get; set; }

        [Display(Name = "Aseguradora")]
        public string Aseguradora { get; set; }
    }
}
