using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models
{
    public class HistoriasClinicas
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public int CitaId { get; set; }
        public int IdMedico { get; set; }
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Required]
        public string MotivoConsulta { get; set; }
        public string EnfermedadActual { get; set; }
        public string? EnfermedadBase { get; set; }

        [Required]
        public string Diagnostico { get; set; }
        public string EvolucionAnalisis { get; set; }
        public string ConductaMedicaRecomendaciones { get; set; }

        // Propiedades de navegación
        public Pacientes Paciente { get; set; }
        public Citas Cita { get; set; }
        public Doctores Medico { get; set; }
        public ICollection<ExamenFisico> ExamenesFisicos { get; set; } = new List<ExamenFisico>();
        public ICollection<ExamenFisicoAdicional> ExamenesFisicosAdicionales { get; set; } = new List<ExamenFisicoAdicional>();
        public ICollection<ProcedimientoProfesional> ProcedimientosProfesionales { get; set; } = new List<ProcedimientoProfesional>();
        public ICollection<VisitaSucesiva> VisitasSucesivas { get; set; } = new List<VisitaSucesiva>();
    }
}
