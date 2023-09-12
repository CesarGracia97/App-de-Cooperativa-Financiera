using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Authorization;

namespace act_Application.Controllers.Administrador.BD
{
    public class ActCuotasController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActCuotasController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActCuotas
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.ActCuotas.ToListAsync());
        }

        // GET: ActCuotas/Create
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActCuotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([Bind("Id,IdUser,IdTransaccion,ValorCuota,FechaCuota,Estado")] ActCuota actCuota)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actCuota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actCuota);
        }

        // GET: ActCuotas/EditTConfirmado/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActCuotas == null)
            {
                return NotFound();
            }

            var actCuota = await _context.ActCuotas.FindAsync(id);
            if (actCuota == null)
            {
                return NotFound();
            }
            return View(actCuota);
        }

        // POST: ActCuotas/EditTConfirmado/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUser,IdTransaccion,ValorCuota,FechaCuota,Estado")] ActCuota actCuota)
        {
            if (id != actCuota.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actCuota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActCuotaExists(actCuota.Id))
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
            return View(actCuota);
        }

        // GET: ActCuotas/Delete/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActCuotas == null)
            {
                return NotFound();
            }

            var actCuota = await _context.ActCuotas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actCuota == null)
            {
                return NotFound();
            }

            return View(actCuota);
        }

        // POST: ActCuotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActCuotas == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActCuotas'  is null.");
            }
            var actCuota = await _context.ActCuotas.FindAsync(id);
            if (actCuota != null)
            {
                _context.ActCuotas.Remove(actCuota);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActCuotaExists(int id)
        {
          return _context.ActCuotas.Any(e => e.Id == id);
        }
    }
}
