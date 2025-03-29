using ConsultaMedica.Data;
using ConsultaMedica.Models;
using ConsultaMedica.Models.HistoriaClinica;
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
            // Obtener la cita con el paciente relacionado
            var cita = _context.citas
                .Include(c => c.Paciente)
                .FirstOrDefault(c => c.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            var doctores = _context.doctores
               .Select(d => new SelectListItem
               {
                   Value = d.Id.ToString(),
                   Text = $"{d.PrimerApellido} {d.SegundoApellido} {d.Nombre}"
               })
               .ToList();
            ViewBag.Doctores = doctores;

            // Obtener el ID del paciente desde la cita
            int idPaciente = cita.Paciente.Id;

            // Obtener todas las historias clínicas del paciente
            var historiasClinicas = _context.historiasClinicas
                .Where(h => h.IdPaciente == idPaciente)
                .Select(h => new
                {
                    h.Id,
                    h.MotivoConsulta,
                    h.Diagnostico,
                    h.ConductaMedicaRecomendaciones,
                    h.FechaAlta,
                    Procedimientos = _context.ProcedimientosProfesionales
                        .Where(p => p.IdHistoriaClinica == h.Id)
                        .Select(p => new
                        {
                            p.NombreProcedimiento,
                            p.NombreProfesional
                        }).ToList()
                }).ToList();

            // Obtener la historia clínica asociada a la cita actual (si existe)
            var historiaExistente = _context.historiasClinicas
                .FirstOrDefault(h => h.CitaId == id);

            // Pasar los datos necesarios a la vista
            ViewBag.Paciente = cita.Paciente;
            ViewBag.CitaId = id;
            ViewBag.MedicoId = User.FindFirstValue(ClaimTypes.NameIdentifier); // ID del médico logueado
            ViewBag.HistoriasClinicas = historiasClinicas;
            ViewBag.HistoriaClinica = historiaExistente;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int citaId, HistoriaClinicaViewModel model)
        {
            try
            {
                // Obtener información de la cita
                var cita = _context.citas
                    .Include(c => c.Paciente)
                    .FirstOrDefault(c => c.Id == citaId);

                if (cita == null)
                {
                    TempData["Mensaje"] = "Cita no encontrada";
                    TempData["TipoMensaje"] = "error";
                    return RedirectToAction("Index", "Agenda"); // Redirige a Agenda
                }

                // Crear historia clínica (permite múltiples historias para el mismo paciente/cita)
                var historiaClinica = new HistoriasClinicas
                {
                    IdPaciente = cita.PacienteId,
                    CitaId = citaId,
                    FechaAlta = DateTime.Now,
                    MotivoConsulta = model.MotivoConsulta ?? "No especificado",
                    EnfermedadActual = model.EnfermedadActual ?? "No especificada",
                    Diagnostico = model.Diagnostico ?? "No especificado",
                    EvolucionAnalisis = model.EvolucionAnalisis ?? "No especificada",
                    ConductaMedicaRecomendaciones = model.ConductaMedica ?? "No especificada",
                    IdMedico = model.IdMedico
                };

                _context.historiasClinicas.Add(historiaClinica);
                _context.SaveChanges();

                // Crear examen físico
                var examenFisico = new ExamenFisico
                {
                    IdHistoriaClinica = historiaClinica.Id,
                    Temperatura = model.Temperatura ?? "No registrada",
                    FrecuenciaCardiaca = model.FrecuenciaCardiaca ?? "No registrada",
                    TensionArterial = model.TensionArterial ?? "No registrada",
                    FrecuenciaRespiratoria = model.FrecuenciaRespiratoria ?? "No registrada",
                    SatO2 = model.SatO2 ?? "No registrada"
                };

                _context.ExamenesFisicos.Add(examenFisico);

                // Crear procedimiento profesional
                var doctor = _context.doctores.FirstOrDefault(d => d.Id == model.IdMedico);

                if (doctor == null)
                {
                    TempData["Mensaje"] = "El médico seleccionado no existe.";
                    TempData["TipoMensaje"] = "error";
                    return RedirectToAction("Index", "Agenda"); // Redirige a Agenda
                }

                var procedimiento = new ProcedimientoProfesionalss
                {
                    IdHistoriaClinica = historiaClinica.Id,
                    NombreProcedimiento = model.NombreProcedimiento ?? "Consulta médica",
                    Observaciones = model.ObservacionProcedimiento ?? "No hay observaciones",
                    FechaProcedimiento = model.FechaProcedimiento ?? DateTime.Now,
                    NombreProfesional = doctor.Nombre
                };

                _context.ProcedimientosProfesionales.Add(procedimiento);
                _context.SaveChanges();

                TempData["Mensaje"] = "Historia clínica guardada correctamente";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction("Index", "Agenda"); // Redirige a Agenda
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al guardar la historia clínica: " + ex.Message;
                TempData["TipoMensaje"] = "error";

                // Recargar datos para la vista si falla
                var citaForView = _context.citas
                    .Include(c => c.Paciente)
                    .FirstOrDefault(c => c.Id == citaId);

                if (citaForView == null) return NotFound();

                ViewBag.Paciente = citaForView.Paciente;
                ViewBag.CitaId = citaId;

                return View("Index", model);
            }
        }
    }
}
