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
            var especialidades = _context.especialidades.ToList();
            ViewBag.Especialidades = new SelectList(especialidades, "Id", "Nombre");

            var pacientes = _context.pacientes
               .Select(p => new SelectListItem
               {
                   Value = p.Id.ToString(),
                   Text = $"{p.PrimerApellido} {p.SegundoApellido} {p.Nombre}"
               })
               .ToList();

            ViewBag.Pacientes = pacientes;
            return View();
        }

        // GET: CitaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

        // GET: CitaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CitaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CitaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CitaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
