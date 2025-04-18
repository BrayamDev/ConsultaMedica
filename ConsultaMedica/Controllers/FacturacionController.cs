using ConsultaMedica.Data;
using ConsultaMedica.Models;
using ConsultaMedica.Models.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
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

        [HttpGet]
        public IActionResult Index(int id)
        {
            // Verificar si ya existe una factura para esta cita
            var facturaExistente = _context.Facturas
                .Include(f => f.Paciente)
                .Include(f => f.TratamientosFacturados)
                .FirstOrDefault(f => f.CitaId == id);

            if (facturaExistente != null)
            {
                var resumen = new FacturaResumenViewModel
                {
                    NumeroFactura = facturaExistente.NumeroFactura,
                    FechaFactura = facturaExistente.FechaFactura,
                    NombrePaciente = facturaExistente.Paciente.NombreCompleto,
                    ImporteTotal = facturaExistente.ImporteTotal,
                    TipoCobro = facturaExistente.TipoCobro,
                    Tratamientos = facturaExistente.TratamientosFacturados.Select(t => new TratamientoResumenViewModel
                    {
                        Nombre = t.NombreTratamiento,
                        Unidades = t.Unidades,
                        PrecioUnitario = t.ImporteUnitario,
                        Total = t.Total
                    }).ToList(),
                    EsFactura = facturaExistente.EsFactura
                };

                ViewBag.FacturaExistente = resumen;
                ViewBag.FacturaCreada = true;
            }

            // Resto de tu código actual...
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

            // Crear el ViewModel y pasarlo a la vista
            var model = new FacturacionViewModel
            {
                CitaId = id,
                PacienteId = cita.PacienteId,
                TratamientosSeleccionados = new List<TratamientoSeleccionado>(),
                TratamientosDisponibles = tratamientosDisponibles
            };

            ViewBag.Paciente = cita.Paciente;
            ViewBag.TratamientosDisponibles = tratamientosDisponibles;

            return View(model);
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

            return Redirect($"~/Facturacion/Index?id={citaId}");
        }
        [HttpPost]
        public IActionResult ProcesarFacturacion(FacturacionViewModel model)
        {

            // Verificar si ya existe una factura para esta cita
            if (_context.Facturas.Any(f => f.CitaId == model.CitaId))
            {
                TempData["ErrorMessage"] = "Esta cita ya tiene una factura asociada.";
                return RedirectToAction("Index", "Agenda");
            }

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
                    ObservacionesCobro = model.ObservacionesCobro ?? string.Empty,
                    ImporteTotal = model.ImporteTotal,
                    PacienteId = model.PacienteId,
                    CitaId = model.CitaId,
                    EsFactura = model.EsFactura // Añadir este campo
                };

                _context.Facturas.Add(factura);
                _context.SaveChanges();

                // 2. Procesar tratamientos
                if (model.TratamientosSeleccionados?.Any() == true)
                {
                    foreach (var tratamientoVM in model.TratamientosSeleccionados)
                    {
                        _context.TratamientosFacturados.Add(new TratamientoFacturado
                        {
                            FacturaId = factura.Id,
                            TratamientoId = tratamientoVM.TratamientoId,
                            NombreTratamiento = tratamientoVM.NombreTratamiento,
                            Unidades = tratamientoVM.Unidades,
                            ImporteUnitario = tratamientoVM.ImporteUnitario,
                            ObservacionesTratamiento = tratamientoVM.Observaciones ?? string.Empty,
                            Total = tratamientoVM.Unidades * tratamientoVM.ImporteUnitario
                        });
                    }
                    _context.SaveChanges();
                }

                // 3. Actualizar estado de la cita
                cita.Facturada = true;
                _context.SaveChanges();

                transaction.Commit();
                TempData.Remove("TratamientosSeleccionados");

                // Generar PDF si está marcado
                if (model.EsFactura)
                {
                    var pdfBytes = GenerarFacturaPdf(factura);
                    TempData["PDFBytes"] = Convert.ToBase64String(pdfBytes);
                }

                var resumen = new FacturaResumenViewModel
                {
                    NumeroFactura = factura.NumeroFactura,
                    FechaFactura = factura.FechaFactura,
                    NombrePaciente = cita.Paciente.NombreCompleto,
                    ImporteTotal = factura.ImporteTotal,
                    TipoCobro = factura.TipoCobro,
                    Tratamientos = factura.TratamientosFacturados.Select(t => new TratamientoResumenViewModel
                    {
                        Nombre = t.NombreTratamiento,
                        Unidades = t.Unidades,
                        PrecioUnitario = t.ImporteUnitario,
                        Total = t.Total
                    }).ToList()
                };

                return View("ResumenFactura", resumen);

            }
            catch (Exception ex)
            {
                ViewBag.Paciente = cita.Paciente;
                ViewBag.TratamientosDisponibles = _context.Tratamientos.ToList();
                TempData["ErrorMessage"] = $"Error al procesar: {ex.Message}";

                return View("Index", model);
            }
        }
        /*Metodos de ayuda*/
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
        private byte[] GenerarFacturaPdf(Factura factura)
        {
            using (var ms = new MemoryStream())
            {
                using (var document = new Document(PageSize.A4, 30, 30, 30, 30))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    // Encabezado
                    var titulo = new Paragraph("FACTURA", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
                    titulo.Alignment = Element.ALIGN_CENTER;
                    document.Add(titulo);

                    document.Add(new Paragraph(" ")); // Espacio

                    // Datos de la factura
                    var tablaDatos = new PdfPTable(2);
                    tablaDatos.WidthPercentage = 100;
                    tablaDatos.SetWidths(new float[] { 1, 2 });

                    // Número y fecha
                    tablaDatos.AddCell("Número de Factura:");
                    tablaDatos.AddCell(factura.NumeroFactura);
                    tablaDatos.AddCell("Fecha:");
                    tablaDatos.AddCell(factura.FechaFactura.ToString("dd/MM/yyyy"));

                    // Datos del paciente
                    tablaDatos.AddCell("Paciente:");
                    tablaDatos.AddCell(factura.Paciente.NombreCompleto);

                    // Método de pago
                    tablaDatos.AddCell("Método de Pago:");
                    tablaDatos.AddCell(factura.TipoCobro);

                    document.Add(tablaDatos);
                    document.Add(new Paragraph(" ")); // Espacio

                    // Tabla de tratamientos
                    var tablaTratamientos = new PdfPTable(4);
                    tablaTratamientos.WidthPercentage = 100;
                    tablaTratamientos.SetWidths(new float[] { 3, 1, 1, 1 });

                    // Encabezados de la tabla
                    tablaTratamientos.AddCell(new Phrase("Tratamiento", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaTratamientos.AddCell(new Phrase("Unidades", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaTratamientos.AddCell(new Phrase("P. Unitario", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));
                    tablaTratamientos.AddCell(new Phrase("Total", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)));

                    // Filas de datos
                    foreach (var tratamiento in factura.TratamientosFacturados)
                    {
                        tablaTratamientos.AddCell(tratamiento.NombreTratamiento);
                        tablaTratamientos.AddCell(tratamiento.Unidades.ToString());
                        tablaTratamientos.AddCell(tratamiento.ImporteUnitario.ToString("C"));
                        tablaTratamientos.AddCell(tratamiento.Total.ToString("C"));
                    }

                    document.Add(tablaTratamientos);
                    document.Add(new Paragraph(" ")); // Espacio

                    // Total
                    var total = new Paragraph($"TOTAL: {factura.ImporteTotal.ToString("C")}",
                        new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD));
                    total.Alignment = Element.ALIGN_RIGHT;
                    document.Add(total);

                    document.Close();
                }
                return ms.ToArray();
            }
        }
        [HttpGet]
        public IActionResult DescargarFactura(string numeroFactura)
        {
            try
            {
                var factura = _context.Facturas
                    .Include(f => f.Paciente)
                    .Include(f => f.TratamientosFacturados)
                    .FirstOrDefault(f => f.NumeroFactura == numeroFactura);

                if (factura == null)
                {
                    TempData["ErrorMessage"] = "Factura no encontrada";
                    return RedirectToAction("Index", "Agenda");
                }

                // Si ya tenemos el PDF generado en TempData (desde ProcesarFacturacion)
                if (TempData["PDFBytes"] is string pdfBase64)
                {
                    var pdfBytes = Convert.FromBase64String(pdfBase64);
                    return File(pdfBytes, "application/pdf", $"Factura-{numeroFactura}.pdf");
                }

                // Generar PDF si no está en TempData
                var newPdfBytes = GenerarFacturaPdf(factura);
                return File(newPdfBytes, "application/pdf", $"Factura-{numeroFactura}.pdf");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al generar PDF: {ex.Message}";
                return RedirectToAction("Index", "Agenda");
            }
        }
    }
}
