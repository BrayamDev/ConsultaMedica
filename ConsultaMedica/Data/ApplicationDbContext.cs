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

        // Tablas existentes
        public DbSet<Citas> citas { get; set; }
        public DbSet<Especialidades> especialidades { get; set; }
        public DbSet<Pacientes> pacientes { get; set; }
        public DbSet<Doctores> doctores { get; set; }

        // Tablas para Historia Clínica (actualizadas)
        public DbSet<HistoriasClinicas> historiasClinicas { get; set; }
        public DbSet<ExamenFisico> ExamenesFisicos { get; set; }
        public DbSet<ExamenFisicoAdicional> ExamenesFisicosAdicionales { get; set; }
        public DbSet<ProcedimientoProfesional> ProcedimientosProfesionales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Citas
            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Especialidad)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.EspecialidadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Citas>()
                .HasOne(c => c.HistoriaClinica)
                .WithOne(h => h.Cita)
                .HasForeignKey<HistoriasClinicas>(h => h.CitaId)
                .IsRequired(false);

            // Configuración de Pacientes
            modelBuilder.Entity<Pacientes>()
                .HasMany(p => p.HistoriasClinicas)
                .WithOne(h => h.Paciente)
                .HasForeignKey(h => h.IdPaciente)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de HistoriasClinicas
            modelBuilder.Entity<HistoriasClinicas>()
                .HasOne(h => h.Medico)
                .WithMany()
                .HasForeignKey(h => h.IdMedico)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de índices
            modelBuilder.Entity<Pacientes>()
                .HasIndex(p => p.NumeroDocumento)
                .IsUnique();

            modelBuilder.Entity<Doctores>()
                .HasIndex(d => d.NumColegiado)
                .IsUnique();
        }
    }
}