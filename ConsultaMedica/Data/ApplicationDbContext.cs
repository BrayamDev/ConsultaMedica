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
        public DbSet<HistoriasClinicas> historiasClinicas { get; set; }
        public DbSet<ExamenFisico> ExamenesFisicos { get; set; }
        public DbSet<ExamenFisicoAdicional> ExamenesFisicosAdicionales { get; set; }
        public DbSet<ProcedimientoProfesional> ProcedimientosProfesionales { get; set; }
        public DbSet<ProcedimientoVisitaSucesiva> procedimientoVisitaSucesivas { get; set; }
        public DbSet<VisitaSucesiva> visitaSucesivas { get; set; }
        public DbSet<TipoDocumento> TiposDocumento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PRIMERO configurar TipoDocumento
            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TiposDocumento");

                // Configuración de la relación inversa
                entity.HasMany(t => t.Pacientes)
                      .WithOne(p => p.TipoDocumento)
                      .HasForeignKey(p => p.TipoDocumentoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // LUEGO configurar Pacientes
            modelBuilder.Entity<Pacientes>(entity =>
            {
                // Esta parte ya no es necesaria porque está definida en TipoDocumento
                // entity.HasOne(p => p.TipoDocumento)... 

                // Configuración EXPLÍCITA del nombre de columna
                entity.Property(p => p.TipoDocumentoId)
                      .HasColumnName("IdTipoDocumento");

                // Restricción única para documento
                entity.HasIndex(p => new { p.TipoDocumentoId, p.NumeroDocumento })
                      .IsUnique();

                entity.HasMany(p => p.HistoriasClinicas)
                      .WithOne(h => h.Paciente)
                      .HasForeignKey(h => h.IdPaciente)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Citas
            modelBuilder.Entity<Citas>(entity =>
            {
                entity.HasOne(c => c.Paciente)
                      .WithMany(p => p.Citas)
                      .HasForeignKey(c => c.PacienteId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Especialidad)
                      .WithMany(e => e.Citas)
                      .HasForeignKey(c => c.EspecialidadId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.HistoriaClinica)
                      .WithOne(h => h.Cita)
                      .HasForeignKey<HistoriasClinicas>(h => h.CitaId)
                      .IsRequired(false);
            });

            // Configuración de Doctores
            modelBuilder.Entity<Doctores>(entity =>
            {
                entity.HasIndex(d => d.NumColegiado)
                      .IsUnique();
            });

            // Configuración de HistoriasClinicas
            modelBuilder.Entity<HistoriasClinicas>(entity =>
            {
                entity.HasOne(h => h.Medico)
                      .WithMany()
                      .HasForeignKey(h => h.IdMedico)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de VisitaSucesiva
            modelBuilder.Entity<VisitaSucesiva>(entity =>
            {
                entity.HasOne(v => v.HistoriaClinica)
                      .WithMany()
                      .HasForeignKey(v => v.IdHistoriaClinica)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.Cita)
                      .WithMany()
                      .HasForeignKey(v => v.IdCita)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.MedicoResponsable)
                      .WithMany()
                      .HasForeignKey(v => v.IdMedicoResponsable)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de ProcedimientoVisitaSucesiva
            modelBuilder.Entity<ProcedimientoVisitaSucesiva>(entity =>
            {
                entity.HasOne(p => p.VisitaSucesiva)
                      .WithMany(v => v.Procedimientos)
                      .HasForeignKey(p => p.IdVisitaSucesiva)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Profesional)
                      .WithMany()
                      .HasForeignKey(p => p.IdProfesional)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}