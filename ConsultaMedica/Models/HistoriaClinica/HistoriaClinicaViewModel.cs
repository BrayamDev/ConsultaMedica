﻿using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models.HistoriaClinica
{
    public class HistoriaClinicaViewModel
    {
        // Datos de historia clínica
        [Required] public string MotivoConsulta { get; set; }
        [Required] public string EnfermedadActual { get; set; }
        [Required] public string Diagnostico { get; set; }
        [Required] public string EvolucionAnalisis { get; set; }
        [Required] public string ConductaMedica { get; set; }

        // Datos de examen físico
        public string Temperatura { get; set; }
        public string FrecuenciaCardiaca { get; set; }
        public string TensionArterial { get; set; }
        public string FrecuenciaRespiratoria { get; set; }
        public string SatO2 { get; set; }

        // Datos de procedimiento profesional
        public string ObservacionProcedimiento { get; set; }
        public string NombreProfesional { get; set; }
        public string NombreProcedimiento { get; set; }
        public DateTime? FechaProcedimiento { get; set; }
        public int IdMedico { get; set; } // Nota el '?' que indica nullable

        // **Nueva Propiedad**
        public string NombreEspecialidad { get; set; }
    }

}
