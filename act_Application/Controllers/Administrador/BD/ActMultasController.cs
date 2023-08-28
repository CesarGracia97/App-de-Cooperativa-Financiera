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
    public class ActMultasController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActMultasController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActMultas
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.ActMultas.ToListAsync());
        }

        // GET: ActMultas/Create
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActMultas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([Bind("Id,IdUser,Porcentaje,Valor,FechaMulta,IdAportacion,Cuadrante1,Cuadrante2")] ActMulta actMulta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actMulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actMulta);
        }

        // GET: ActMultas/Edit/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActMultas == null)
            {
                return NotFound();
            }

            var actMulta = await _context.ActMultas.FindAsync(id);
            if (actMulta == null)
            {
                return NotFound();
            }
            return View(actMulta);
        }

        // POST: ActMultas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUser,Porcentaje,Valor,FechaMulta,IdAportacion,Cuadrante1,Cuadrante2")] ActMulta actMulta)
        {
            if (id != actMulta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actMulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActMultaExists(actMulta.Id))
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
            return View(actMulta);
        }

        // GET: ActMultas/Delete/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActMultas == null)
            {
                return NotFound();
            }

            var actMulta = await _context.ActMultas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actMulta == null)
            {
                return NotFound();
            }

            return View(actMulta);
        }

        // POST: ActMultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActMultas == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActMultas'  is null.");
            }
            var actMulta = await _context.ActMultas.FindAsync(id);
            if (actMulta != null)
            {
                _context.ActMultas.Remove(actMulta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActMultaExists(int id)
        {
          return _context.ActMultas.Any(e => e.Id == id);
        }
    }
}
