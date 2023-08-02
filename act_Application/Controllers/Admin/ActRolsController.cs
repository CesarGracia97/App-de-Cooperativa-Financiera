using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using act_Application.Models;
using act_Application.Data.Data;
using Microsoft.AspNetCore.Authorization;

namespace act_Application.Controllers.Admin
{
    public class ActRolsController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActRolsController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActRols
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
            return _context.ActRols != null ?
                        View(await _context.ActRols.ToListAsync()) :
                        Problem("Entity set 'DesarrolloContext.ActRols'  is null.");
        }

        // GET: ActRols/Details/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActRols == null)
            {
                return NotFound();
            }

            var actRol = await _context.ActRols
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actRol == null)
            {
                return NotFound();
            }

            return View(actRol);
        }

        // GET: ActRols/Create
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActRols/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreRol,DescripcionRol")] ActRol actRol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actRol);
        }

        // GET: ActRols/Edit/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActRols == null)
            {
                return NotFound();
            }

            var actRol = await _context.ActRols.FindAsync(id);
            if (actRol == null)
            {
                return NotFound();
            }
            return View(actRol);
        }

        // POST: ActRols/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreRol,DescripcionRol")] ActRol actRol)
        {
            if (id != actRol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actRol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActRolExists(actRol.Id))
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
            return View(actRol);
        }

        // GET: ActRols/Delete/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActRols == null)
            {
                return NotFound();
            }

            var actRol = await _context.ActRols
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actRol == null)
            {
                return NotFound();
            }

            return View(actRol);
        }

        // POST: ActRols/Delete/5
        [Authorize(Policy = "AdminOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActRols == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActRols'  is null.");
            }
            var actRol = await _context.ActRols.FindAsync(id);
            if (actRol != null)
            {
                _context.ActRols.Remove(actRol);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActRolExists(int id)
        {
            return (_context.ActRols?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
