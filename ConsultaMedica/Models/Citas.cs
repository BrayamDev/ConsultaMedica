using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class Citas
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        public int EspecialidadId { get; set; }

        [Required(ErrorMessage = "El tiempo de visita es obligatorio.")]
        public int TiempoVisita { get; set; }

        public int PacienteId { get; set; }
        public string Observaciones { get; set; }
        public bool Estado { get; set; }
        public bool Facturada { get; set; }

        // Propiedades de navegación
        public Pacientes Paciente { get; set; }
        public Especialidades Especialidad { get; set; }
        public ICollection<HistoriasClinicas> HistoriasClinicas { get; set; } = new List<HistoriasClinicas>();
    }
}
