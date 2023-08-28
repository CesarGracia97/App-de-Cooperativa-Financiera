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
    public class ActTransaccionesController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActTransaccionesController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActTransacciones
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.ActTransacciones.ToListAsync());
        }

        // GET: ActTransacciones/Create
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActTransacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actTransaccione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actTransaccione);
        }

        // GET: ActTransacciones/Edit/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActTransacciones == null)
            {
                return NotFound();
            }

            var actTransaccione = await _context.ActTransacciones.FindAsync(id);
            if (actTransaccione == null)
            {
                return NotFound();
            }
            return View(actTransaccione);
        }

        // POST: ActTransacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione)
        {
            if (id != actTransaccione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActTransaccioneExists(actTransaccione.Id))
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
            return View(actTransaccione);
        }

        // GET: ActTransacciones/Delete/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActTransacciones == null)
            {
                return NotFound();
            }

            var actTransaccione = await _context.ActTransacciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actTransaccione == null)
            {
                return NotFound();
            }

            return View(actTransaccione);
        }

        // POST: ActTransacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActTransacciones == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActTransacciones'  is null.");
            }
            var actTransaccione = await _context.ActTransacciones.FindAsync(id);
            if (actTransaccione != null)
            {
                _context.ActTransacciones.Remove(actTransaccione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActTransaccioneExists(int id)
        {
          return _context.ActTransacciones.Any(e => e.Id == id);
        }
    }
}
