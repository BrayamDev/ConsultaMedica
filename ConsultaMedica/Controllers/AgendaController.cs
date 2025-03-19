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
        public ActionResult Index()
        {
            // Obtener la lista de especialidades
            var especialidades = _context.especialidades.ToList();
            ViewBag.Especialidades = new SelectList(especialidades, "Id", "Nombre");

            // Obtener la lista de pacientes
            var pacientes = _context.pacientes
               .Select(p => new SelectListItem
               {
                   Value = p.Id.ToString(),
                   Text = $"{p.PrimerApellido} {p.SegundoApellido} {p.Nombre}"
               })
               .ToList();
            ViewBag.Pacientes = pacientes;

            // Obtener la lista de doctores
            var doctores = _context.doctores
               .Select(d => new SelectListItem
               {
                   Value = d.Id.ToString(),
                   Text = $"{d.PrimerApellido} {d.SegundoApellido} {d.Nombre}"
               })
               .ToList();
            ViewBag.Doctores = doctores;

            return View();
        }

        // Método para obtener las citas en formato JSON
        public IActionResult GetCitas()
        {
            var citas = _context.citas
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
                _context.citas.Add(cita);
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




    }
}
