﻿// <auto-generated />
using System;
using ConsultaMedica.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConsultaMedica.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250416203722_InitialMigrate")]
    partial class InitialMigrate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConsultaMedica.Models.Citas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EspecialidadId")
                        .HasColumnType("int");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<DateTime>("FechaHora")
                        .HasColumnType("datetime2");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PacienteId")
                        .HasColumnType("int");

                    b.Property<int>("TiempoVisita")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EspecialidadId");

                    b.HasIndex("PacienteId");

                    b.ToTable("Citas");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Doctores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Acronimo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EspecialidadPrincipal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fotografia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Movil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumColegiado")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NumeroDeDocumento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimerApellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SegundoApellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoDeDocumento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NumColegiado")
                        .IsUnique();

                    b.ToTable("Doctores");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Especialidades", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Especialidades");
                });

            modelBuilder.Entity("ConsultaMedica.Models.ExamenFisico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FrecuenciaCardiaca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FrecuenciaRespiratoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdHistoriaClinica")
                        .HasColumnType("int");

                    b.Property<string>("ObservacionesExamenFisico")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SatO2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Temperatura")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TensionArterial")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdHistoriaClinica");

                    b.ToTable("ExamenesFisicos");
                });

            modelBuilder.Entity("ConsultaMedica.Models.ExamenFisicoAdicional", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdHistoriaClinica")
                        .HasColumnType("int");

                    b.Property<string>("NombreItem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdHistoriaClinica");

                    b.ToTable("ExamenesFisicosAdicionales");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Factura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CitaId")
                        .HasColumnType("int");

                    b.Property<string>("Empresa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaFactura")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("ImporteTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("NumeroFactura")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservacionesCobro")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("PacienteId")
                        .HasColumnType("int");

                    b.Property<string>("TipoCobro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TratamientoId")
                        .HasColumnType("int");

                    b.Property<int>("Unidad")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CitaId");

                    b.HasIndex("PacienteId");

                    b.HasIndex("TratamientoId");

                    b.ToTable("Facturas");
                });

            modelBuilder.Entity("ConsultaMedica.Models.HistoriasClinicas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CitaId")
                        .HasColumnType("int");

                    b.Property<string>("ConductaMedicaRecomendaciones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Diagnostico")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnfermedadActual")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnfermedadBase")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvolucionAnalisis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaAlta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("IdMedico")
                        .HasColumnType("int");

                    b.Property<int>("IdPaciente")
                        .HasColumnType("int");

                    b.Property<string>("MotivoConsulta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CitaId");

                    b.HasIndex("IdMedico");

                    b.HasIndex("IdPaciente");

                    b.ToTable("HistoriasClinicas");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Pacientes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Aseguradora")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CodigoPostal")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Direccion")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Movil")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NumeroDocumento")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Observaciones")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaisOrigen")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Poblacion")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PrimerApellido")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Procedencia")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Provincia")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SegundoApellido")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Sexo")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Telefono")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("TipoDocumentoId")
                        .HasColumnType("int")
                        .HasColumnName("IdTipoDocumento");

                    b.HasKey("Id");

                    b.HasIndex("NumeroDocumento")
                        .IsUnique()
                        .HasFilter("[NumeroDocumento] IS NOT NULL");

                    b.HasIndex("TipoDocumentoId");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("ConsultaMedica.Models.ProcedimientoProfesional", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaProcedimiento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("IdHistoriaClinica")
                        .HasColumnType("int");

                    b.Property<string>("NombreProcedimiento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreProfesional")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdHistoriaClinica");

                    b.ToTable("ProcedimientosProfesionales");
                });

            modelBuilder.Entity("ConsultaMedica.Models.ProcedimientoVisitaSucesiva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaProcedimiento")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdVisitaSucesiva")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProfesionalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdVisitaSucesiva");

                    b.HasIndex("ProfesionalId");

                    b.ToTable("ProcedimientosVisitaSucesiva");
                });

            modelBuilder.Entity("ConsultaMedica.Models.TipoDocumento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("TiposDocumento");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Tratamiento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("EspecialidadId")
                        .HasColumnType("int");

                    b.Property<decimal>("ImporteUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("NombreTratamiento")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("Codigo")
                        .IsUnique();

                    b.HasIndex("EspecialidadId");

                    b.ToTable("Tratamientos");
                });

            modelBuilder.Entity("ConsultaMedica.Models.TratamientoFacturado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FacturaId")
                        .HasColumnType("int");

                    b.Property<decimal>("ImporteUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("NombreTratamiento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservacionesTratamiento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TratamientoId")
                        .HasColumnType("int");

                    b.Property<int>("Unidades")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FacturaId");

                    b.HasIndex("TratamientoId");

                    b.ToTable("TratamientosFacturados");
                });

            modelBuilder.Entity("ConsultaMedica.Models.VisitaSucesiva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConductaMedica")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvolucionAnalisis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime>("FechaVisita")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("IdCita")
                        .HasColumnType("int");

                    b.Property<int>("IdHistoriaClinica")
                        .HasColumnType("int");

                    b.Property<int>("IdMedicoResponsable")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdCita");

                    b.HasIndex("IdHistoriaClinica");

                    b.ToTable("VisitasSucesivas");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Citas", b =>
                {
                    b.HasOne("ConsultaMedica.Models.Especialidades", "Especialidad")
                        .WithMany("Citas")
                        .HasForeignKey("EspecialidadId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ConsultaMedica.Models.Pacientes", "Paciente")
                        .WithMany("Citas")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Especialidad");

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("ConsultaMedica.Models.ExamenFisico", b =>
                {
                    b.HasOne("ConsultaMedica.Models.HistoriasClinicas", "HistoriaClinica")
                        .WithMany("ExamenesFisicos")
                        .HasForeignKey("IdHistoriaClinica")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HistoriaClinica");
                });

            modelBuilder.Entity("ConsultaMedica.Models.ExamenFisicoAdicional", b =>
                {
                    b.HasOne("ConsultaMedica.Models.HistoriasClinicas", "HistoriaClinica")
                        .WithMany("ExamenesFisicosAdicionales")
                        .HasForeignKey("IdHistoriaClinica")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HistoriaClinica");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Factura", b =>
                {
                    b.HasOne("ConsultaMedica.Models.Citas", "Cita")
                        .WithMany()
                        .HasForeignKey("CitaId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ConsultaMedica.Models.Pacientes", "Paciente")
                        .WithMany("Facturas")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ConsultaMedica.Models.Tratamiento", "Tratamiento")
                        .WithMany()
                        .HasForeignKey("TratamientoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Cita");

                    b.Navigation("Paciente");

                    b.Navigation("Tratamiento");
                });

            modelBuilder.Entity("ConsultaMedica.Models.HistoriasClinicas", b =>
                {
                    b.HasOne("ConsultaMedica.Models.Citas", "Cita")
                        .WithMany("HistoriasClinicas")
                        .HasForeignKey("CitaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsultaMedica.Models.Doctores", "Medico")
                        .WithMany()
                        .HasForeignKey("IdMedico")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ConsultaMedica.Models.Pacientes", "Paciente")
                        .WithMany("HistoriasClinicas")
                        .HasForeignKey("IdPaciente")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cita");

                    b.Navigation("Medico");

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Pacientes", b =>
                {
                    b.HasOne("ConsultaMedica.Models.TipoDocumento", "TipoDocumento")
                        .WithMany("Pacientes")
                        .HasForeignKey("TipoDocumentoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TipoDocumento");
                });

            modelBuilder.Entity("ConsultaMedica.Models.ProcedimientoProfesional", b =>
                {
                    b.HasOne("ConsultaMedica.Models.HistoriasClinicas", "HistoriaClinica")
                        .WithMany("ProcedimientosProfesionales")
                        .HasForeignKey("IdHistoriaClinica")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HistoriaClinica");
                });

            modelBuilder.Entity("ConsultaMedica.Models.ProcedimientoVisitaSucesiva", b =>
                {
                    b.HasOne("ConsultaMedica.Models.VisitaSucesiva", "VisitaSucesiva")
                        .WithMany("Procedimientos")
                        .HasForeignKey("IdVisitaSucesiva")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsultaMedica.Models.Doctores", null)
                        .WithMany()
                        .HasForeignKey("ProfesionalId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("VisitaSucesiva");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Tratamiento", b =>
                {
                    b.HasOne("ConsultaMedica.Models.Especialidades", "Especialidad")
                        .WithMany("Tratamientos")
                        .HasForeignKey("EspecialidadId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Especialidad");
                });

            modelBuilder.Entity("ConsultaMedica.Models.TratamientoFacturado", b =>
                {
                    b.HasOne("ConsultaMedica.Models.Factura", "Factura")
                        .WithMany("TratamientosFacturados")
                        .HasForeignKey("FacturaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsultaMedica.Models.Tratamiento", "Tratamiento")
                        .WithMany()
                        .HasForeignKey("TratamientoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Factura");

                    b.Navigation("Tratamiento");
                });

            modelBuilder.Entity("ConsultaMedica.Models.VisitaSucesiva", b =>
                {
                    b.HasOne("ConsultaMedica.Models.Citas", "Cita")
                        .WithMany()
                        .HasForeignKey("IdCita")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ConsultaMedica.Models.HistoriasClinicas", "HistoriaClinica")
                        .WithMany("VisitasSucesivas")
                        .HasForeignKey("IdHistoriaClinica")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cita");

                    b.Navigation("HistoriaClinica");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Citas", b =>
                {
                    b.Navigation("HistoriasClinicas");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Especialidades", b =>
                {
                    b.Navigation("Citas");

                    b.Navigation("Tratamientos");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Factura", b =>
                {
                    b.Navigation("TratamientosFacturados");
                });

            modelBuilder.Entity("ConsultaMedica.Models.HistoriasClinicas", b =>
                {
                    b.Navigation("ExamenesFisicos");

                    b.Navigation("ExamenesFisicosAdicionales");

                    b.Navigation("ProcedimientosProfesionales");

                    b.Navigation("VisitasSucesivas");
                });

            modelBuilder.Entity("ConsultaMedica.Models.Pacientes", b =>
                {
                    b.Navigation("Citas");

                    b.Navigation("Facturas");

                    b.Navigation("HistoriasClinicas");
                });

            modelBuilder.Entity("ConsultaMedica.Models.TipoDocumento", b =>
                {
                    b.Navigation("Pacientes");
                });

            modelBuilder.Entity("ConsultaMedica.Models.VisitaSucesiva", b =>
                {
                    b.Navigation("Procedimientos");
                });
#pragma warning restore 612, 618
        }
    }
}
