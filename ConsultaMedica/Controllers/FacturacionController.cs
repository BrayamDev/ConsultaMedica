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
        public IActionResult Index(int id)
        {
            // Obtener la cita con el paciente relacionado
            var cita = _context.Citas
                .Include(c => c.Paciente)
                .FirstOrDefault(c => c.Id == id);

            if (cita == null)
            {
                return NotFound();
            }

            // Verificar si el paciente existe y tiene historia clínica
            var pacienteExiste = _context.Pacientes.Any(p => p.Id == cita.PacienteId);
            var tieneHistoriaClinica = _context.HistoriasClinicas.Any(h => h.IdPaciente == cita.PacienteId);

            if (!pacienteExiste || !tieneHistoriaClinica)
            {
                TempData["ErrorMessage"] = "No se puede acceder a facturación: El paciente no existe o no tiene historia clínica registrada";
                return RedirectToAction("Index", "Agenda");
            }

            // Obtener todos los tratamientos disponibles con sus especialidades
            var tratamientosDisponibles = _context.Tratamientos
                .Include(t => t.Especialidad)
                .OrderBy(t => t.NombreTratamiento)
                .ToList();

            // Recuperar tratamientos seleccionados desde TempData (si existen)
            List<TratamientoSeleccionado> tratamientosSeleccionados = new();
            if (TempData["TratamientosSeleccionados"] != null)
            {
                tratamientosSeleccionados = JsonConvert.DeserializeObject<List<TratamientoSeleccionado>>(
                    TempData["TratamientosSeleccionados"].ToString()
                );
                // Reestablecer en TempData por si necesitas acceder nuevamente
                TempData.Keep("TratamientosSeleccionados");
            }

            // Crear el ViewModel y pasarlo a la vista
            var model = new FacturacionViewModel
            {
                CitaId = id,
                TratamientosSeleccionados = tratamientosSeleccionados,
                TratamientosDisponibles = tratamientosDisponibles
            };

            ViewBag.Paciente = cita.Paciente;
            ViewBag.TratamientosDisponibles = tratamientosDisponibles;

            return View(model);
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
        [HttpGet]
        public IActionResult EliminarTratamientoSeleccionado(int tratamientoId, int citaId)
        {
            var tratamientosJson = TempData["TratamientosSeleccionados"] as string;

            if (string.IsNullOrEmpty(tratamientosJson))
                return RedirectToAction("Index");

            var tratamientos = JsonConvert.DeserializeObject<List<TratamientoSeleccionado>>(tratamientosJson);

            tratamientos = tratamientos.Where(t => t.TratamientoId != tratamientoId).ToList();

            TempData["TratamientosSeleccionados"] = JsonConvert.SerializeObject(tratamientos);
            TempData.Keep("TratamientosSeleccionados");

            // 5. Redireccionar con el ID de la cita encontrada
            return Redirect($"~/Facturacion/Index?id={citaId}");
        }
        [HttpPost]
        public IActionResult ProcesarFacturacion(FacturacionViewModel model)
        {
            // Recargar datos necesarios si hay algún error posterior
            var cita = _context.Citas
                .Include(c => c.Paciente)
                .FirstOrDefault(c => c.Id == model.CitaId);

            if (cita == null)
            {
                return NotFound();
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // 1. Crear la factura principal
                var factura = new Factura
                {
                    NumeroFactura = GenerarNumeroFactura(),
                    FechaFactura = model.FechaFactura,
                    Empresa = model.Empresa,
                    TipoCobro = model.TipoCobro,
                    ObservacionesCobro = string.IsNullOrEmpty(model.ObservacionesCobro) ? "" : model.ObservacionesCobro,
                    ImporteTotal = model.ImporteTotal,
                    PacienteId = model.PacienteId,
                    CitaId = model.CitaId
                };

                _context.Facturas.Add(factura);
                _context.SaveChanges();

                // 2. Procesar los tratamientos seleccionados
                if (model.TratamientosSeleccionados != null && model.TratamientosSeleccionados.Any())
                {
                    foreach (var tratamientoVM in model.TratamientosSeleccionados)
                    {
                        var tratamientoFacturado = new TratamientoFacturado
                        {
                            FacturaId = factura.Id,
                            TratamientoId = tratamientoVM.TratamientoId,
                            NombreTratamiento = tratamientoVM.NombreTratamiento,
                            Unidades = tratamientoVM.Unidades,
                            ImporteUnitario = tratamientoVM.ImporteUnitario,
                            ObservacionesTratamiento = string.IsNullOrEmpty(tratamientoVM.Observaciones) ? "" : tratamientoVM.Observaciones,
                            Total = tratamientoVM.Unidades * tratamientoVM.ImporteUnitario
                        };

                        _context.TratamientosFacturados.Add(tratamientoFacturado);
                    }

                    _context.SaveChanges();
                }

                // 3. Actualizar la cita como facturada
                if (cita != null)
                {
                    cita.Facturada = true;
                    _context.SaveChanges();
                }

                transaction.Commit();

                // Limpiar los tratamientos seleccionados
                TempData.Remove("TratamientosSeleccionados");

                // Configurar ViewBag para la vista de éxito
                ViewBag.NumeroFactura = factura.NumeroFactura;
                ViewBag.FechaFactura = factura.FechaFactura.ToString("dd/MM/yyyy");
                ViewBag.ImporteTotal = factura.ImporteTotal.ToString("C");

                return View("FacturaGenerada", factura);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // Loggear el error (ex)

                // Recargar datos necesarios para mostrar la vista nuevamente
                ViewBag.Paciente = cita.Paciente;
                ViewBag.TratamientosDisponibles = _context.Tratamientos.ToList();

                TempData["ErrorMessage"] = "Ocurrió un error al procesar la factura. Por favor, inténtelo de nuevo.";
                return View("Index", model);
            }
        }
        private string GenerarNumeroFactura()
        {
            var now = DateTime.Now;
            var ultimaFactura = _context.Facturas
                .OrderByDescending(f => f.Id)
                .FirstOrDefault();

            int secuencia = 1;
            if (ultimaFactura != null)
            {
                secuencia = int.Parse(ultimaFactura.NumeroFactura.Split('-').Last()) + 1;
            }

            return $"FAC-{now:yyyyMMdd}-{secuencia:D4}";
        }
    }
}
