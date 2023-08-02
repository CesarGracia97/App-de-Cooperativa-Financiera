using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using act_Application.Models;
using act_Application.Data.Data;

namespace act_Application.Controllers.Admin
{
    public class ActMultasController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActMultasController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActMultas
        public async Task<IActionResult> Index()
        {
            return _context.ActMultas != null ?
                        View(await _context.ActMultas.ToListAsync()) :
                        Problem("Entity set 'DesarrolloContext.ActMultas'  is null.");
        }

        // GET: ActMultas/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ActMultas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActMultas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,Porcentaje")] ActMulta actMulta)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUser,Porcentaje")] ActMulta actMulta)
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
            return (_context.ActMultas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
