using ConsultaMedica.Data;
using ConsultaMedica.Models;
using ConsultaMedica.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ConsultaMedica.Controllers
{
    public class HistoriaClinicaController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HistoriaClinicaController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(int id)
        {
            ViewBag.Title = $"Historia Clínica";

            // Obtener la cita con el paciente relacionado
            var cita = _context.Citas
                .Include(c => c.Paciente)
                .FirstOrDefault(c => c.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            var doctores = _context.Doctores
               .Select(d => new SelectListItem
               {
                   Value = d.Id.ToString(),
                   Text = $"{d.PrimerApellido} {d.SegundoApellido} {d.Nombre}"
               })
               .ToList();
            ViewBag.Doctores = doctores;

            // Obtener el ID del paciente desde la cita
            int idPaciente1 = cita.Paciente.Id;

            var historiasClinicas = _context.HistoriasClinicas
            .Where(h => h.IdPaciente == idPaciente1)
            .OrderByDescending(h => h.FechaAlta)
            .Select(h => new
            {
                h.Id,
                MotivoConsulta = h.MotivoConsulta ?? string.Empty,
                EnfermedadBase = h.EnfermedadBase ?? string.Empty, // Manejo explícito de NULL
                EnfermedadActual = h.EnfermedadActual ?? string.Empty,
                Diagnostico = h.Diagnostico ?? string.Empty,
                EvolucionAnalisis = h.EvolucionAnalisis ?? string.Empty,
                ConductaMedicaRecomendaciones = h.ConductaMedicaRecomendaciones ?? string.Empty,
                h.FechaAlta,
                h.CitaId,
                ExamenFisico = _context.ExamenesFisicos
                    .Where(e => e.IdHistoriaClinica == h.Id)
                    .Select(e => new
                    {
                        Temperatura = (string)e.Temperatura,
                        FrecuenciaCardiaca = (string)e.FrecuenciaCardiaca,
                        TensionArterial = (string)e.TensionArterial ?? string.Empty,
                        FrecuenciaRespiratoria = (string)e.FrecuenciaRespiratoria,
                        SatO2 = (string)e.SatO2
                    }).FirstOrDefault(),
                Procedimientos = _context.ProcedimientosProfesionales
                    .Where(p => p.IdHistoriaClinica == h.Id)
                    .Select(p => new
                    {
                        p.NombreProcedimiento,
                        NombreProfesional = p.NombreProfesional ?? string.Empty,
                        Observaciones = p.Observaciones ?? string.Empty,
                        p.FechaProcedimiento
                    }).ToList(),
                VisitasSucesivas = _context.VisitasSucesivas
                    .Where(v => v.IdHistoriaClinica == h.Id)
                    .Select(v => new
                    {
                        v.Id,
                        v.FechaVisita,
                        EvolucionAnalisis = v.EvolucionAnalisis ?? string.Empty,
                        ConductaMedica = v.ConductaMedica ?? string.Empty,
                        MedicoResponsable = _context.Doctores
                            .Where(d => d.Id == v.IdMedicoResponsable)
                            .Select(d => $"{d.Nombre ?? ""} {d.PrimerApellido ?? ""}")
                            .FirstOrDefault() ?? string.Empty,
                        Procedimientos = v.Procedimientos.Select(p => new
                        {
                            p.FechaProcedimiento,
                            Observaciones = p.Observaciones ?? string.Empty,
                            Profesional = _context.Doctores
                                .Where(d => d.Id == p.ProfesionalId)
                                .Select(d => $"{d.Nombre ?? ""} {d.PrimerApellido ?? ""}")
                                .FirstOrDefault() ?? string.Empty
                        }).ToList()
                    }).ToList()
            }).ToList();

            var historiaExistente = _context.HistoriasClinicas
                .FirstOrDefault(h => h.CitaId == id);

            ViewBag.Paciente = cita.Paciente;
            ViewBag.CitaId = id;
            ViewBag.MedicoId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.HistoriasClinicas = historiasClinicas;
            ViewBag.HistoriaClinica = historiaExistente;
            ViewBag.IdHistoriaClinica = historiaExistente?.Id;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int citaId, HistoriaClinicaViewModel model)
        {
            try
            {
                var cita = _context.Citas
                    .Include(c => c.Paciente)
                    .FirstOrDefault(c => c.Id == citaId);

                if (cita == null)
                {
                    TempData["Mensaje"] = "Cita no encontrada";
                    TempData["TipoMensaje"] = "error";
                    return RedirectToAction("Index", "Agenda");
                }

                // Validar procedimientos
                if (model.Procedimientos != null)
                {
                    foreach (var proc in model.Procedimientos)
                    {
                        if (string.IsNullOrWhiteSpace(proc.NombreProcedimiento))
                        {
                            ModelState.AddModelError("", "El nombre del procedimiento es requerido");
                        }

                        if (proc.IdMedico == 0)
                        {
                            ModelState.AddModelError("", "Debe seleccionar un médico para cada procedimiento");
                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Paciente = cita.Paciente;
                    ViewBag.CitaId = citaId;
                    ViewBag.Doctores = _context.Doctores
                        .Select(d => new SelectListItem
                        {
                            Value = d.Id.ToString(),
                            Text = $"{d.PrimerApellido} {d.SegundoApellido} {d.Nombre}"
                        })
                        .ToList();

                    return View("Index", model);
                }

                // Crear historia clínica
                var historiaClinica = new HistoriasClinicas
                {
                    IdPaciente = cita.PacienteId,
                    CitaId = citaId,
                    FechaAlta = cita.FechaHora,
                    MotivoConsulta = model.MotivoConsulta ?? "No especificado",
                    EnfermedadActual = model.EnfermedadActual ?? "No especificada",
                    Diagnostico = model.Diagnostico ?? "No especificado",
                    EvolucionAnalisis = model.EvolucionAnalisis ?? "No especificada",
                    ConductaMedicaRecomendaciones = model.ConductaMedica ?? "No especificada",
                    IdMedico = model.IdMedico,
                    EnfermedadBase = model.EnfermedadBase ?? String.Empty,
      
                };

                _context.HistoriasClinicas.Add(historiaClinica);
                _context.SaveChanges();

                // Crear examen físico
                var examenFisico = new ExamenFisico
                {
                    IdHistoriaClinica = historiaClinica.Id,
                    Temperatura = model.Temperatura ?? "No registrada",
                    FrecuenciaCardiaca = model.FrecuenciaCardiaca ?? "No registrada",
                    TensionArterial = model.TensionArterial ?? "No registrada",
                    FrecuenciaRespiratoria = model.FrecuenciaRespiratoria ?? "No registrada",
                    SatO2 = model.SatO2 ?? "No registrada",
                    ObservacionesExamenFisico = model.ObservacionesExamenFisico
                };

                _context.ExamenesFisicos.Add(examenFisico);

                // Guardar procedimientos
                if (model.Procedimientos != null && model.Procedimientos.Any())
                {
                    foreach (var proc in model.Procedimientos)
                    {
                        var doctor = _context.Doctores.FirstOrDefault(d => d.Id == proc.IdMedico);

                        if (doctor == null)
                        {
                            continue;
                        }

                        var procedimiento = new ProcedimientoProfesional
                        {
                            IdHistoriaClinica = historiaClinica.Id,
                            NombreProcedimiento = proc.NombreProcedimiento ?? "Procedimiento médico",
                            Observaciones = proc.Observaciones ?? "No hay observaciones",
                            FechaProcedimiento = proc.FechaProcedimiento ?? DateTime.Now,
                            NombreProfesional = $"{doctor.Nombre} {doctor.PrimerApellido}",
                            
                        };
                        _context.ProcedimientosProfesionales.Add(procedimiento);
                    }
                }

                _context.SaveChanges();

                TempData["Mensaje"] = "Historia clínica guardada correctamente";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction("Index", "Agenda");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al guardar la historia clínica: {ex.Message}";
                TempData["TipoMensaje"] = "error";

                var citaForView = _context.Citas
                    .Include(c => c.Paciente)
                    .FirstOrDefault(c => c.Id == citaId);

                if (citaForView == null) return NotFound();

                ViewBag.Paciente = citaForView.Paciente;
                ViewBag.CitaId = citaId;
                ViewBag.Doctores = _context.Doctores
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = $"{d.PrimerApellido} {d.SegundoApellido} {d.Nombre}"
                    })
                    .ToList();

                return View("Index", model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarVisitaSucesiva(VisitaSucesivaViewModel model)
        {
            try
            {
                // Obtener la cita con el paciente relacionado
                var cita = _context.Citas
                    .Include(c => c.Paciente)
                    .FirstOrDefault(c => c.Id == model.IdCita);

                if (cita == null)
                {
                    return NotFound();
                }

                // Obtener la historia clínica más reciente del paciente con el médico asociado
                var historiaClinica = _context.HistoriasClinicas
                    .Include(h => h.Medico) // Asegúrate de incluir el médico
                    .Where(h => h.IdPaciente == cita.Paciente.Id)
                    .OrderByDescending(h => h.FechaAlta)
                    .FirstOrDefault();

                if (historiaClinica == null)
                {
                    TempData["Mensaje"] = "Historia clínica no encontrada";
                    TempData["TipoMensaje"] = "error";
                    return RedirectToAction("Index", "HistoriaClinica", new { id = model.IdCita });
                }

                // Asignar el ID de la historia clínica y el médico al modelo
                model.IdHistoriaClinica = historiaClinica.Id;
                var idMedicoHistoria = historiaClinica.IdMedico; // Médico que creó la historia clínica

                // Validaciones (mantén las que ya tienes)...

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Crear registro principal de visita sucesiva
                        var visitaSucesiva = new VisitaSucesiva
                        {
                            IdHistoriaClinica = model.IdHistoriaClinica,
                            IdCita = model.IdCita,
                            FechaVisita = DateTime.Now,
                            EvolucionAnalisis = model.EvolucionAnalisis,
                            ConductaMedica = model.ConductaMedica,
                            IdMedicoResponsable = idMedicoHistoria, // Usamos el médico de la historia
                            FechaCreacion = DateTime.Now
                        };

                        _context.VisitasSucesivas.Add(visitaSucesiva);
                        _context.SaveChanges();

                        // Agregar procedimientos - TODOS con el mismo médico de la historia clínica
                        if (model.Procedimientos != null && model.Procedimientos.Any())
                        {
                            foreach (var proc in model.Procedimientos)
                            {
                                var procedimiento = new ProcedimientoVisitaSucesiva
                                {
                                    IdVisitaSucesiva = visitaSucesiva.Id,
                                    Observaciones = proc.Observaciones ?? "No hay observaciones",
                                    FechaProcedimiento = proc.FechaProcedimiento.Value,
                                    ProfesionalId = proc.IdProfesional
                                };

                                _context.ProcedimientosVisitaSucesiva.Add(procedimiento);
                            }
                        }

                        _context.SaveChanges();
                        transaction.Commit();

                        TempData["Mensaje"] = "Visita sucesiva registrada correctamente";
                        TempData["TipoMensaje"] = "success";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        TempData["Mensaje"] = $"Error al registrar: {ex.Message}";
                        TempData["TipoMensaje"] = "error";
                        return RedirectToAction("Index", "HistoriaClinica", new { id = model.IdCita });
                    }
                }

                return RedirectToAction("Index", "HistoriaClinica", new { id = model.IdCita });
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error general: {ex.Message}";
                TempData["TipoMensaje"] = "error";
                return RedirectToAction("Index", "HistoriaClinica", new { id = model.IdCita });
            }
        }

    }
}
