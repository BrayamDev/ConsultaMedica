using ConsultaMedica.Data;
using ConsultaMedica.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ConsultaMedica.Controllers
{
    public class AgendaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgendaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CitaController
        public ActionResult Index(int? id)
        {
            // Código existente para dropdowns
            var especialidades = _context.Especialidades.ToList();
            ViewBag.Especialidades = new SelectList(especialidades, "Id", "Nombre");

            var pacientes = _context.Pacientes
               .Select(p => new SelectListItem
               {
                   Value = p.Id.ToString(),
                   Text = $"{p.PrimerApellido} {p.SegundoApellido} {p.Nombre}"
               })
               .ToList();
            ViewBag.Pacientes = pacientes;

            var doctores = _context.Doctores
               .Select(d => new SelectListItem
               {
                   Value = d.Id.ToString(),
                   Text = $"{d.PrimerApellido} {d.SegundoApellido} {d.Nombre}"
               })
               .ToList();
            ViewBag.Doctores = doctores;


            var tiposDocumento = _context.TiposDocumento
               .Select(d => new SelectListItem
               {
                   Value = d.Id.ToString(),
                   Text = $"{d.Nombre}"
               })
               .ToList();
            ViewBag.TiposDocumento = tiposDocumento;

            // Nuevo código para manejar el ID de paciente
            if (id.HasValue)
            {
                var cita = _context.Citas
                    .Include(c => c.Paciente)
                    .FirstOrDefault(c => c.Id == id.Value);

                if (cita != null)
                {
                    ViewBag.PacienteActual = cita.Paciente;
                    ViewBag.CitaId = id.Value;
                }
            }

            return View();
        }


        // Método para obtener las citas en formato JSON
        public IActionResult GetCitas()
        {
            var citas = _context.Citas
                .Include(c => c.Paciente) // Incluye la relación con Paciente
                .Select(c => new
                {
                    id = c.Id, // Identificador de la cita
                    title = $"Cita con {c.Paciente.Nombre}", // Usa el nombre del paciente
                    start = c.FechaHora.ToString("yyyy-MM-ddTHH:mm:ss"), // Fecha y hora de inicio
                    end = c.FechaHora.AddMinutes(c.TiempoVisita).ToString("yyyy-MM-ddTHH:mm:ss"), // Fecha y hora de fin
                    observaciones = c.Observaciones,
                    especialidadId = c.EspecialidadId
                })
                .ToList();

            return Json(citas);
        }

        // GET: CitaController/Create
        public ActionResult Create()
        { 
            return View();
        }

        // POST: CitaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Citas cita)
        {
            try
            {
                // Guardar la cita en la base de datos
                _context.Citas.Add(cita);
                _context.SaveChanges();

                // Enviar un mensaje de éxito usando TempData
                TempData["Mensaje"] = "La cita se ha insertado correctamente.";
                TempData["TipoMensaje"] = "success"; // Para identificar el tipo de mensaje
            }
            catch (Exception ex)
            {
                // Enviar un mensaje de error si algo sale mal
                TempData["Mensaje"] = "Error al insertar la cita: " + ex.Message;
                TempData["TipoMensaje"] = "error"; // Para identificar el tipo de mensaje
            }

            // Redirigir a la vista Index de Agenda
            return RedirectToAction("Index", "Agenda");
        }
        [HttpGet]
        public ActionResult ConsultarTipoDocumento()
        {
            // Cargar solo tipos de documento en ViewBag
            var tiposDocumento = _context.TiposDocumento
                .Where(t => t.Activo)
                .OrderBy(t => t.Nombre)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Nombre
                })
                .ToList();

            ViewBag.TiposDocumento = tiposDocumento;

            return View();
        }

        [HttpGet]
        public IActionResult EditarCitas(int id)
        {
            var cita = _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Especialidad)
                .FirstOrDefault(c => c.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            ViewBag.Especialidades = _context.Especialidades
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Nombre
                })
                .ToList();

            return View(cita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarCitasPost(Citas cita)
        {
        
            try
            {
                // Verificar si la cita existe
                var citaExistente = _context.Citas.Find(cita.Id);
                if (citaExistente == null)
                {
                    return NotFound();
                }

                // Actualizar los campos necesarios
                citaExistente.FechaHora = cita.FechaHora;
                citaExistente.EspecialidadId = cita.EspecialidadId;
                citaExistente.TiempoVisita = cita.TiempoVisita;
                citaExistente.Observaciones = cita.Observaciones;

                _context.Update(citaExistente);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Cita actualizada correctamente";
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al actualizar la cita: " + ex.Message);
            }
            
            ViewBag.Especialidades = _context.Especialidades
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Nombre
                })
                .ToList();

            return View("EditarCitas", cita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarCita(int id)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // 1. Eliminar los procedimientos de las visitas sucesivas
                var procedimientos = _context.ProcedimientosVisitaSucesiva
                    .Where(p => p.VisitaSucesiva.IdCita == id)
                    .ToList();

                _context.ProcedimientosVisitaSucesiva.RemoveRange(procedimientos);

                // 2. Eliminar las visitas sucesivas
                var visitasSucesivas = _context.VisitasSucesivas
                    .Where(v => v.IdCita == id)
                    .ToList();

                _context.VisitasSucesivas.RemoveRange(visitasSucesivas);

                // 3. Finalmente eliminar la cita principal
                var cita = _context.Citas.Find(id);
                if (cita == null)
                {
                    return NotFound();
                }

                _context.Citas.Remove(cita);

                _context.SaveChanges();
                transaction.Commit();

                TempData["SuccessMessage"] = "Cita, visitas relacionadas y procedimientos eliminados correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
                return RedirectToAction("EditarCitas", new { id });
            }
        }
    }
}
