using ConsultaMedica.Data;
using ConsultaMedica.Models;
using ConsultaMedica.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ConsultaMedica.Controllers
{
    public class FacturacionController : Controller
    {

        private readonly ApplicationDbContext _context;

        public FacturacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FacturacionController
        public IActionResult Index(int id) // ID de la cita
        {
            // Obtener la cita con el paciente relacionado
            var cita = _context.Citas
                .Include(c => c.Paciente)
                .FirstOrDefault(c => c.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            // Obtener todos los tratamientos disponibles con sus especialidades
            var tratamientosDisponibles = _context.Tratamientos
                .Include(t => t.Especialidad)  // Incluir la relación con Especialidad
                .OrderBy(t => t.NombreTratamiento)        // Ordenar alfabéticamente
                .ToList();

            // Pasar los datos a la vista
            ViewBag.Paciente = cita.Paciente;
            ViewBag.TratamientosDisponibles = tratamientosDisponibles;

            return View();
        }

        [HttpPost]
        public IActionResult AgregarTratamientos(int PacienteId, int CitaId, List<int> tratamientosSeleccionados)
        {
            // 1. Obtener la última cita del paciente (o la lógica que necesites)
            var cita = _context.Citas
                .Where(c => c.PacienteId == PacienteId)
                .OrderByDescending(c => c.FechaHora) // Obtener la más reciente
                .FirstOrDefault();

            if (cita == null)
            {
                return NotFound("No se encontró cita para este paciente");
            }

            // 2. Obtener los tratamientos seleccionados
            var tratamientos = _context.Tratamientos
                .Where(t => tratamientosSeleccionados.Contains(t.Id))
                .Include(t => t.Especialidad)
                .ToList();

            // 3. Mapear a ViewModel
            var tratamientosVM = tratamientos.Select(t => new TratamientoSeleccionado
            {
                TratamientoId = t.Id,
                NombreTratamiento = t.NombreTratamiento,
                Codigo = t.Codigo,
                ImporteUnitario = t.ImporteUnitario,
                Unidades = 1
            }).ToList();

            // 4. Guardar en TempData
            TempData["TratamientosSeleccionados"] = JsonConvert.SerializeObject(tratamientosVM);

            // 5. Redireccionar con el ID de la cita encontrada
            return Redirect($"~/Facturacion/Index?id={CitaId}");
        }
    }
}
