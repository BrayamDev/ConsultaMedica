using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models.ViewModels
{
    public class CitaPacienteViewModel
    {
        // Propiedades de Cita
        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        public int EspecialidadId { get; set; }

        [Required(ErrorMessage = "El tiempo de visita es obligatorio.")]
        public int TiempoVisita { get; set; } = 30; // Valor por defecto

        public string ObservacionesCita { get; set; }
        public bool EstadoCita { get; set; } = true; // Valor por defecto

        // Propiedades de Paciente
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string PrimerApellido { get; set; }

        [StringLength(100, ErrorMessage = "El segundo apellido no puede exceder 100 caracteres")]
        public string SegundoApellido { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [StringLength(20)]
        public string Sexo { get; set; } = "Masculino";

        [StringLength(100)]
        public string PaisOrigen { get; set; }

        [StringLength(100)]
        public string Provincia { get; set; }

        [StringLength(100)]
        public string Poblacion { get; set; }

        public int TipoDocumentoId { get; set; }

        [StringLength(50)]
        public string NumeroDocumento { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [StringLength(20)]
        public string CodigoPostal { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(20)]
        public string Movil { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Procedencia { get; set; }

        [StringLength(100)]
        public string Aseguradora { get; set; }

        public string ObservacionesPaciente { get; set; }

        // Control para nuevo paciente
        public bool EsNuevoPaciente { get; set; }

        // Para selección de paciente existente
        public int? PacienteId { get; set; }

        // Listas para dropdowns
        public List<SelectListItem> Especialidades { get; set; }
        public List<SelectListItem> TiposDocumento { get; set; }
        public List<SelectListItem> Pacientes { get; set; }
    }
}
