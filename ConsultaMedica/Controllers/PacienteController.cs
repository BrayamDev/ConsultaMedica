using ConsultaMedica.Data;
using ConsultaMedica.Models;
using ConsultaMedica.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var listaPacientes = _context.Pacientes.ToList();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearPacienteCita(CitaPacienteViewModel model)
        {
            try
            {
                // Validación adicional para documento
                if (model.TipoDocumentoId.HasValue && string.IsNullOrWhiteSpace(model.NumeroDocumento))
                {
                    ModelState.AddModelError("NumeroDocumento", "Debe ingresar un número de documento cuando selecciona un tipo");
                }

                // Validar documento único si se proporcionó
                if (!string.IsNullOrWhiteSpace(model.NumeroDocumento) && model.TipoDocumentoId.HasValue)
                {
                    var existeDocumento = _context.Pacientes
                        .Any(p => p.NumeroDocumento == model.NumeroDocumento &&
                                 p.TipoDocumentoId == model.TipoDocumentoId);

                    if (existeDocumento)
                    {
                        ModelState.AddModelError("NumeroDocumento", "Ya existe un paciente con este número de documento");
                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.TiposDocumento = _context.TiposDocumento
                        .Select(t => new SelectListItem
                        {
                            Value = t.Id.ToString(),
                            Text = t.Nombre
                        })
                        .ToList();

                    TempData["TipoMensaje"] = "error";
                    TempData["Mensaje"] = "Por favor corrija los errores en el formulario";
                    return View(model);
                }

                // Crear el paciente
                var paciente = new Pacientes
                {
                    Nombre = model.Nombre,
                    PrimerApellido = model.PrimerApellido,
                    SegundoApellido = model.SegundoApellido,
                    FechaNacimiento = model.FechaNacimiento,
                    Sexo = model.Sexo ?? "No especificado",
                    PaisOrigen = model.PaisOrigen,
                    Provincia = model.Provincia,
                    Poblacion = model.Poblacion,
                    TipoDocumentoId = (int)model.TipoDocumentoId,
                    NumeroDocumento = model.NumeroDocumento,
                    Direccion = model.Direccion,
                    CodigoPostal = model.CodigoPostal,
                    Telefono = model.Telefono,
                    Movil = model.Movil,
                    Email = model.Email,
                    Procedencia = model.Procedencia,
                    Aseguradora = model.Aseguradora,
                };

                _context.Pacientes.Add(paciente);
                _context.SaveChanges();


                TempData["Mensaje"] = "Paciente registrado correctamente";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction("Index", "Agenda");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al guardar el paciente: {ex.Message}";
                TempData["TipoMensaje"] = "error";

                ViewBag.TiposDocumento = _context.TiposDocumento
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Nombre
                    })
                    .ToList();

                return View(model);
            }
        }
    }
}
