using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsultaMedica.Models.ViewModels
{
    // ViewModels/FacturacionViewModel.cs
    public class FacturacionViewModel
    {
        // Datos del paciente y cita
        public int PacienteId { get; set; }
        public int CitaId { get; set; }

        // Datos de facturación
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaFactura { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La empresa es requerida")]
        public string Empresa { get; set; } = "Consulta médica domiciliaria";

        public bool EsFactura { get; set; }
        public decimal ImporteTotal { get; set; }

        // Datos de cobro
        [Required(ErrorMessage = "El tipo de cobro es requerido")]
        public string TipoCobro { get; set; } = "efectivo";
        public string? ObservacionesCobro { get; set; }

        // Tratamientos
        public List<TratamientoSeleccionado> TratamientosSeleccionados { get; set; } = new();
        public List<Tratamiento> TratamientosDisponibles { get; set; }

    }



    public class TratamientoSeleccionado
    {
        public int TratamientoId { get; set; }
        public string Codigo { get; set; }
        public string NombreTratamiento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Las unidades deben ser al menos 1")]
        public int Unidades { get; set; } = 1;

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
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
