namespace ConsultaMedica.Models
{
    public class Pacientes
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public string? PaisOrigen { get; set; }
        public string? Provincia { get; set; }
        public string? Poblacion { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Direccion { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Telefono { get; set; }
        public string? Movil { get; set; }
        public string? Email { get; set; }
        public string? Procedencia { get; set; }
        public string? Aseguradora { get; set; }
        public string? Observaciones { get; set; }

        // Propiedades de navegación
        public ICollection<HistoriasClinicas> HistoriasClinicas { get; set; } = new List<HistoriasClinicas>();
        public ICollection<Citas> Citas { get; set; } = new List<Citas>();


    }
}
