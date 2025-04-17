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

        public IActionResult Index()
        {
            var listaPacientes = _context.Pacientes.ToList();
            ViewBag.Pacientes = listaPacientes;

            return View(listaPacientes);
        }
        public IActionResult ConsolidadoPacientes()
        {
            return View();
        }
        public IActionResult CrearPacienteCita()
        {
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
        public IActionResult DetallePaciente(int id)
        {
            var paciente = _context.Pacientes
                .FirstOrDefault(p => p.Id == id);

            if (paciente == null)
            {
                return NotFound();
            }

            // Asignar todos los datos al ViewBag
            ViewBag.Id = paciente.Id;
            ViewBag.NombreCompleto = $"{paciente.Nombre} {paciente.PrimerApellido} {paciente.SegundoApellido}";
            ViewBag.FechaNacimiento = paciente.FechaNacimiento;
            ViewBag.Sexo = paciente.Sexo;
            ViewBag.NumeroDocumento = paciente.NumeroDocumento;
            ViewBag.Direccion = paciente.Direccion;
            ViewBag.CodigoPostal = paciente.CodigoPostal;
            ViewBag.Poblacion = paciente.Poblacion;
            ViewBag.Provincia = paciente.Provincia;
            ViewBag.PaisOrigen = paciente.PaisOrigen;
            ViewBag.Telefono = paciente.Telefono;
            ViewBag.Movil = paciente.Movil;
            ViewBag.Email = paciente.Email;
            ViewBag.Procedencia = paciente.Procedencia;
            ViewBag.Aseguradora = paciente.Aseguradora;
            ViewBag.Observaciones = paciente.Observaciones;

            return View();
        }
    }
}
