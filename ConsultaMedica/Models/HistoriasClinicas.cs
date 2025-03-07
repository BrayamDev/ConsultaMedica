namespace ConsultaMedica.Models
{
    public class HistoriasClinicas
    {
        public int Id { get; set; }
        public int? IdPaciente { get; set; }
        public DateTime? FechaAlta { get; set; }
        public Pacientes? Paciente { get; set; }
    }
}
