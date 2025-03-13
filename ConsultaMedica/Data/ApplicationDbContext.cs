using ConsultaMedica.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConsultaMedica.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Citas> citas { get; set; }
        public DbSet<Especialidades> especialidades { get; set; }
        public DbSet<HistoriasClinicas> historiasClinicas { get; set; }
        public DbSet<Pacientes> pacientes { get; set; }
        public DbSet<Doctores> doctores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 👈 Agregar esta línea

            modelBuilder.Entity<Pacientes>()
                .HasOne(p => p.HistoriaClinica)
                .WithOne(hc => hc.Paciente)
                .HasForeignKey<HistoriasClinicas>(hc => hc.IdPaciente)
                .IsRequired();

            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.PacienteId);

            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Especialidades)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.EspecialidadId);
        }

    }
}
