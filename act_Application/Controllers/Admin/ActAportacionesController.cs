using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using act_Application.Models;
using act_Application.Data.Data;
using Microsoft.AspNetCore.Authorization;
using act_Application.Logica.ComplementosLogicos;

namespace act_Application.Controllers.Admin
{
    public class ActAportacionesController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActAportacionesController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActAportaciones
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
            return _context.ActAportaciones != null ?
                        View(await _context.ActAportaciones.ToListAsync()) :
                        Problem("Entity set 'DesarrolloContext.ActAportaciones'  is null.");
        }

        // GET: ActAportaciones/Details/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActAportaciones == null)
            {
                return NotFound();
            }

            var actAportacione = await _context.ActAportaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actAportacione == null)
            {
                return NotFound();
            }

            return View(actAportacione);
        }

        // GET: ActAportaciones/Create
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActAportaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Razon,Valor,IdUser,FechaAportacion,Aprobacion,CapturaPantalla,Cuadrante1,Cuadrante2,CBancaria,NBanco")] ActAportacione actAportacione)
        {
            if (ModelState.IsValid)
            {
                // Calcula los cuadrantes antes de guardar los datos
                ObtenerCuadrante.CalcularCuadrantesAportacione(actAportacione);
                _context.Add(actAportacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actAportacione);
        }

        // GET: ActAportaciones/Edit/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActAportaciones == null)
            {
                return NotFound();
            }

            var actAportacione = await _context.ActAportaciones.FindAsync(id);
            if (actAportacione == null)
            {
                return NotFound();
            }
            return View(actAportacione);
        }

        // POST: ActAportaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Razon,Valor,IdUser,FechaAportacion,Aprobacion,CapturaPantalla,Cuadrante1,Cuadrante2,CBancaria,NBanco")] ActAportacione actAportacione)
        {
            if (id != actAportacione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Calcula los cuadrantes antes de guardar los datos
                ObtenerCuadrante.CalcularCuadrantesAportacione(actAportacione);

                try
                {
                    _context.Update(actAportacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActAportacioneExists(actAportacione.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actAportacione);
        }

        // GET: ActAportaciones/Delete/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActAportaciones == null)
            {
                return NotFound();
            }

            var actAportacione = await _context.ActAportaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actAportacione == null)
            {
                return NotFound();
            }

            return View(actAportacione);
        }

        // POST: ActAportaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActAportaciones == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActAportaciones'  is null.");
            }
            var actAportacione = await _context.ActAportaciones.FindAsync(id);
            if (actAportacione != null)
            {
                _context.ActAportaciones.Remove(actAportacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActAportacioneExists(int id)
        {
            return (_context.ActAportaciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
