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
    public class ActNotificacionesController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActNotificacionesController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActNotificaciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActNotificaciones.ToListAsync());
        }

        // GET: ActNotificaciones/Create
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActNotificaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino")] ActNotificacione actNotificacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actNotificacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actNotificacione);
        }

        // GET: ActNotificaciones/EditTConfirmado/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActNotificaciones == null)
            {
                return NotFound();
            }

            var actNotificacione = await _context.ActNotificaciones.FindAsync(id);
            if (actNotificacione == null)
            {
                return NotFound();
            }
            return View(actNotificacione);
        }

        // POST: ActNotificaciones/EditTConfirmado/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino")] ActNotificacione actNotificacione)
        {
            if (id != actNotificacione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actNotificacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActNotificacioneExists(actNotificacione.Id))
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
            return View(actNotificacione);
        }

        // GET: ActNotificaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActNotificaciones == null)
            {
                return NotFound();
            }

            var actNotificacione = await _context.ActNotificaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actNotificacione == null)
            {
                return NotFound();
            }

            return View(actNotificacione);
        }

        // POST: ActNotificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActNotificaciones == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActNotificaciones'  is null.");
            }
            var actNotificacione = await _context.ActNotificaciones.FindAsync(id);
            if (actNotificacione != null)
            {
                _context.ActNotificaciones.Remove(actNotificacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActNotificacioneExists(int id)
        {
            return _context.ActNotificaciones.Any(e => e.Id == id);
        }
    }
}
