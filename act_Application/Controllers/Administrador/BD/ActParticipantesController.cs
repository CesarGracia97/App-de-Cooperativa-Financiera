using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeGenerator.Models.BD;
using act_Application.Data.Data;

namespace act_Application.Controllers.Administrador.BD
{
    public class ActParticipantesController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActParticipantesController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActParticipantes
        public async Task<IActionResult> Index()
        {
              return View(await _context.ActParticipantes.ToListAsync());
        }

        // GET: ActParticipantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActParticipantes == null)
            {
                return NotFound();
            }

            var actParticipante = await _context.ActParticipantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actParticipante == null)
            {
                return NotFound();
            }

            return View(actParticipante);
        }

        // GET: ActParticipantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActParticipantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaInicio,FechaFinalizacion,FechaGeneracion,Participantes")] ActParticipante actParticipante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actParticipante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actParticipante);
        }

        // GET: ActParticipantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActParticipantes == null)
            {
                return NotFound();
            }

            var actParticipante = await _context.ActParticipantes.FindAsync(id);
            if (actParticipante == null)
            {
                return NotFound();
            }
            return View(actParticipante);
        }

        // POST: ActParticipantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaInicio,FechaFinalizacion,FechaGeneracion,Participantes")] ActParticipante actParticipante)
        {
            if (id != actParticipante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actParticipante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActParticipanteExists(actParticipante.Id))
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
            return View(actParticipante);
        }

        // GET: ActParticipantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActParticipantes == null)
            {
                return NotFound();
            }

            var actParticipante = await _context.ActParticipantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actParticipante == null)
            {
                return NotFound();
            }

            return View(actParticipante);
        }

        // POST: ActParticipantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActParticipantes == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActParticipantes'  is null.");
            }
            var actParticipante = await _context.ActParticipantes.FindAsync(id);
            if (actParticipante != null)
            {
                _context.ActParticipantes.Remove(actParticipante);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActParticipanteExists(int id)
        {
          return _context.ActParticipantes.Any(e => e.Id == id);
        }
    }
}
