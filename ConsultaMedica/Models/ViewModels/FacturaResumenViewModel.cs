namespace ConsultaMedica.Models.ViewModels
{
    public class FacturaResumenViewModel
    {
        public string NumeroFactura { get; set; }
        public DateTime FechaFactura { get; set; }
        public string NombrePaciente { get; set; }
        public decimal ImporteTotal { get; set; }
        public string TipoCobro { get; set; }
        public bool EsFactura { get; set; }
        public List<TratamientoResumenViewModel> Tratamientos { get; set; }
    }

    public class TratamientoResumenViewModel
    {
        public string Nombre { get; set; }
        public int Unidades { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
