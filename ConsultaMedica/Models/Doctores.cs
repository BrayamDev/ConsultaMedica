namespace ConsultaMedica.Models
{
    public class Doctores
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Acronimo { get; set; }
        public string NumColegiado { get; set; }
        public string EspecialidadPrincipal { get; set; }
        public string Fotografia { get; set; }
        public string TipoDeDocumento { get; set; }
        public string NumeroDeDocumento { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string Email { get; set; }

        //// Propiedades de navegación añadidas
        //public ICollection<HistoriasClinicas> HistoriasClinicas { get; set; } = new List<HistoriasClinicas>();
        //public ICollection<VisitaSucesiva> VisitasSucesivas { get; set; } = new List<VisitaSucesiva>();
        //public ICollection<ProcedimientoVisitaSucesiva> ProcedimientosVisitaSucesiva { get; set; } = new List<ProcedimientoVisitaSucesiva>();
    }
}
