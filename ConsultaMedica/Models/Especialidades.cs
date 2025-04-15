namespace ConsultaMedica.Models
{
    public class Especialidades
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<Citas> Citas { get; set; } = new List<Citas>();
        public virtual ICollection<Tratamiento> Tratamientos { get; set; }
    }
}
