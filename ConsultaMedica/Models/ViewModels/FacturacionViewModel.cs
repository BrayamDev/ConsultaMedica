using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models.ViewModels
{
    // ViewModels/FacturacionViewModel.cs
    public class FacturacionViewModel
    {
        // Datos del paciente
        public int PacienteId { get; set; }
        public Pacientes Paciente { get; set; } // Se agrega la propiedad del paciente

        // Datos de facturación
        public string? Empresa { get; set; }
        public bool EsFactura { get; set; }
        public string TipoCobro { get; set; } = "efectivo";
        public string? ObservacionesCobro { get; set; }

        // Tratamientos
        public List<Tratamiento> TratamientosDisponibles { get; set; } = new List<Tratamiento>();
        public List<TratamientoSeleccionado> TratamientosSeleccionados { get; set; } = new List<TratamientoSeleccionado>();

        // Calculados
        public decimal ImporteTotal => TratamientosSeleccionados.Sum(t => t.Total);
    }

    public class TratamientoSeleccionado
    {
        public int TratamientoId { get; set; }
        public string Codigo { get; set; }
        public string NombreTratamiento { get; set; }
        public int Unidades { get; set; } = 1;
        public decimal ImporteUnitario { get; set; }
        public string? Observaciones { get; set; }
        public decimal Total => ImporteUnitario * Unidades;
    }

    public class PacientesFacturacion
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Email { get; set; }
    }
}
