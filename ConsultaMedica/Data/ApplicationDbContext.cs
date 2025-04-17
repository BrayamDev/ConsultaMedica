using Microsoft.EntityFrameworkCore;
using ConsultaMedica.Models;

namespace ConsultaMedica.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Citas> Citas { get; set; }
        public DbSet<Doctores> Doctores { get; set; }
        public DbSet<Especialidades> Especialidades { get; set; }
        public DbSet<ExamenFisico> ExamenesFisicos { get; set; }
        public DbSet<ExamenFisicoAdicional> ExamenesFisicosAdicionales { get; set; }
        public DbSet<HistoriasClinicas> HistoriasClinicas { get; set; }
        public DbSet<Pacientes> Pacientes { get; set; }
        public DbSet<ProcedimientoProfesional> ProcedimientosProfesionales { get; set; }
        public DbSet<ProcedimientoVisitaSucesiva> ProcedimientosVisitaSucesiva { get; set; }
        public DbSet<TipoDocumento> TiposDocumento { get; set; }
        public DbSet<VisitaSucesiva> VisitasSucesivas { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<TratamientoFacturado> TratamientosFacturados { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Citas
            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Citas>()
                .HasOne(c => c.Especialidad)
                .WithMany(e => e.Citas)
                .HasForeignKey(c => c.EspecialidadId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Citas>()
                .HasMany(c => c.HistoriasClinicas)
                .WithOne(h => h.Cita)
                .HasForeignKey(h => h.CitaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de Historias Clinicas
            modelBuilder.Entity<HistoriasClinicas>()
                .HasOne(h => h.Paciente)
                .WithMany(p => p.HistoriasClinicas)
                .HasForeignKey(h => h.IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoriasClinicas>()
                .HasOne(h => h.Medico)
                .WithMany()
                .HasForeignKey(h => h.IdMedico)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HistoriasClinicas>()
                .HasMany(h => h.ExamenesFisicos)
                .WithOne(e => e.HistoriaClinica)
                .HasForeignKey(e => e.IdHistoriaClinica)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HistoriasClinicas>()
                .HasMany(h => h.ExamenesFisicosAdicionales)
                .WithOne(e => e.HistoriaClinica)
                .HasForeignKey(e => e.IdHistoriaClinica)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HistoriasClinicas>()
                .HasMany(h => h.ProcedimientosProfesionales)
                .WithOne(p => p.HistoriaClinica)
                .HasForeignKey(p => p.IdHistoriaClinica)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HistoriasClinicas>()
                .HasMany(h => h.VisitasSucesivas)
                .WithOne(v => v.HistoriaClinica)
                .HasForeignKey(v => v.IdHistoriaClinica)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de Visitas Sucesivas
            modelBuilder.Entity<VisitaSucesiva>()
                .HasOne(v => v.Cita)
                .WithMany()
                .HasForeignKey(v => v.IdCita)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VisitaSucesiva>()
                .HasMany(v => v.Procedimientos)
                .WithOne(p => p.VisitaSucesiva)
                .HasForeignKey(p => p.IdVisitaSucesiva)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de Pacientes
            modelBuilder.Entity<Pacientes>()
                .HasOne(p => p.TipoDocumento)
                .WithMany(t => t.Pacientes)
                .HasForeignKey(p => p.TipoDocumentoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de índices únicos
            modelBuilder.Entity<Pacientes>()
                .HasIndex(p => p.NumeroDocumento)
                .IsUnique();

            modelBuilder.Entity<Doctores>()
                .HasIndex(d => d.NumColegiado)
                .IsUnique();

            // Configuración de valores por defecto
            modelBuilder.Entity<Especialidades>()
                .Property(e => e.Estado)
                .HasDefaultValue(true);

            modelBuilder.Entity<Especialidades>()
                .Property(e => e.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<HistoriasClinicas>()
                .Property(h => h.FechaAlta)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ProcedimientoProfesional>()
                .Property(p => p.FechaProcedimiento)
                .HasDefaultValueSql("GETDATE()");

            // Configuración para ProcedimientoVisitaSucesiva
            modelBuilder.Entity<ProcedimientoVisitaSucesiva>()
                .HasOne<Doctores>()  // Especificamos el tipo directamente
                .WithMany()
                .HasForeignKey(p => p.ProfesionalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VisitaSucesiva>()
                .Property(v => v.FechaVisita)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<VisitaSucesiva>()
                .Property(v => v.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            // Configuración de Tratamientos
            modelBuilder.Entity<Tratamiento>()
                .HasOne(t => t.Especialidad)
                .WithMany(e => e.Tratamientos)
                .HasForeignKey(t => t.EspecialidadId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice único para código de tratamiento
            modelBuilder.Entity<Tratamiento>()
                .HasIndex(t => t.Codigo)
                .IsUnique();
            // Configuración de Factura
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Paciente)
                .WithMany(p => p.Facturas)
                .HasForeignKey(f => f.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cita)
                .WithMany()
                .HasForeignKey(f => f.CitaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}