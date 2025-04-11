using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models.ViewModels
{
    public class HistoriaClinicaViewModel
    {
        // Datos de historia clínica
        public int IdCita { get; set; }
        public int IdHistoriaClinica { get; set; }
        [Required] public string MotivoConsulta { get; set; }
        [Required] public string EnfermedadActual { get; set; }
        [Required] public string Diagnostico { get; set; }
        [Required] public string EvolucionAnalisis { get; set; }
        [Required] public string ConductaMedica { get; set; }
        public string? EnfermedadBase { get; set; }
        [Required] public DateTime FechaAlta { get; set; }

        // Datos de examen físico
        public string Temperatura { get; set; }
        public string FrecuenciaCardiaca { get; set; }
        public string TensionArterial { get; set; }
        public string FrecuenciaRespiratoria { get; set; }
        public string SatO2 { get; set; }
        public int IdMedico { get; set; }
        public string ObservacionesExamenFisico { get; set; }

        public List<ProcedimientoViewModel> Procedimientos { get; set; } = new List<ProcedimientoViewModel>();
    }


    public class VisitaSucesivaViewModel
    {
        public int IdHistoriaClinica { get; set; }
        public int IdCita { get; set; }
        public string EvolucionAnalisis { get; set; }
        public string ConductaMedica { get; set; }
        public List<ProcedimientoViewModel> Procedimientos { get; set; }
    }


    public class ProcedimientoViewModel
    {
        public DateTime? FechaProcedimiento { get; set; }
        public string Observaciones { get; set; }
        public int IdMedico { get; set; }
        public int IdProfesional { get; set; }
        public string NombreProcedimiento { get; set; }
    }
}