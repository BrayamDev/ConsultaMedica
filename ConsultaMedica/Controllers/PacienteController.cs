using ConsultaMedica.Data;
using ConsultaMedica.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaMedica.Controllers
{
    public class PacienteController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PacienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PacienteController
        public IActionResult Index()
        {
            var listaPacientes = _context.pacientes.ToList();
            ViewBag.Pacientes = listaPacientes;

            return View(listaPacientes);
        }

        // GET: PacienteController/Details/5
        public IActionResult ConsolidadoPacientes()
        {
            return View();
        }

        // GET: Paciente/CrearPacienteCita
        public IActionResult CrearPacienteCita()
        {
            // Puedes agregar lógica de inicialización aquí si es necesario
            return View();
        }

        // POST: Paciente/CrearPacienteCita
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPacienteCita(Pacientes paciente)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    // Agregar el paciente a la base de datos
                    _context.pacientes.Add(paciente);
                    await _context.SaveChangesAsync();

                    // Redirigir a alguna vista de éxito o a la lista de pacientes
                    TempData["SuccessMessage"] = "Paciente creado exitosamente";
                    return RedirectToAction("Index", "Home"); // Cambia esto por tu acción deseada
                }

                // Si el modelo no es válido, regresar a la vista con los errores
                return View(paciente);
            }
            catch (Exception ex)
            {
                // Loggear el error (puedes implementar un sistema de logging)
                ModelState.AddModelError("", "Ocurrió un error al crear el paciente. Por favor intente nuevamente.");
                return View(paciente);
            }
        }
    }
}
