using ConsultaMedica.Data;
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

        // GET: PacienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PacienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: PacienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PacienteController/Edit/5
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

        // GET: PacienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PacienteController/Delete/5
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
