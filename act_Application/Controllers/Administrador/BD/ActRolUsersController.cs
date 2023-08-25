using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;

namespace act_Application.Controllers.Administrador.BD
{
    public class ActRolUsersController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActRolUsersController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActRolUsers
        public async Task<IActionResult> Index()
        {
              return View(await _context.ActRolUsers.ToListAsync());
        }

        // GET: ActRolUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActRolUsers == null)
            {
                return NotFound();
            }

            var actRolUser = await _context.ActRolUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actRolUser == null)
            {
                return NotFound();
            }

            return View(actRolUser);
        }

        // GET: ActRolUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActRolUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,IdRol")] ActRolUser actRolUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actRolUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actRolUser);
        }

        // GET: ActRolUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActRolUsers == null)
            {
                return NotFound();
            }

            var actRolUser = await _context.ActRolUsers.FindAsync(id);
            if (actRolUser == null)
            {
                return NotFound();
            }
            return View(actRolUser);
        }

        // POST: ActRolUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUser,IdRol")] ActRolUser actRolUser)
        {
            if (id != actRolUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actRolUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActRolUserExists(actRolUser.Id))
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
            return View(actRolUser);
        }

        // GET: ActRolUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActRolUsers == null)
            {
                return NotFound();
            }

            var actRolUser = await _context.ActRolUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actRolUser == null)
            {
                return NotFound();
            }

            return View(actRolUser);
        }

        // POST: ActRolUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActRolUsers == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActRolUsers'  is null.");
            }
            var actRolUser = await _context.ActRolUsers.FindAsync(id);
            if (actRolUser != null)
            {
                _context.ActRolUsers.Remove(actRolUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActRolUserExists(int id)
        {
          return _context.ActRolUsers.Any(e => e.Id == id);
        }
    }
}
