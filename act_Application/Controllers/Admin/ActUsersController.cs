﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using act_Application.Models;

namespace act_Application.Controllers
{
    public class ActUsersController : Controller
    {
        private readonly DesarrolloContext _context;

        public ActUsersController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: ActUsers
        public async Task<IActionResult> Index()
        {
              return _context.ActUsers != null ? 
                          View(await _context.ActUsers.ToListAsync()) :
                          Problem("Entity set 'DesarrolloContext.ActUsers'  is null.");
        }

        // GET: ActUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActUsers == null)
            {
                return NotFound();
            }

            var actUser = await _context.ActUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actUser == null)
            {
                return NotFound();
            }

            return View(actUser);
        }

        // GET: ActUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cedula,Cbancaria,Correo,NombreYapellido,Celular,Contrasena,TipoUser,Ncaccionario")] ActUser actUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actUser);
        }

        // GET: ActUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActUsers == null)
            {
                return NotFound();
            }

            var actUser = await _context.ActUsers.FindAsync(id);
            if (actUser == null)
            {
                return NotFound();
            }
            return View(actUser);
        }

        // POST: ActUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cedula,Cbancaria,Correo,NombreYapellido,Celular,Contrasena,TipoUser,Ncaccionario")] ActUser actUser)
        {
            if (id != actUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActUserExists(actUser.Id))
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
            return View(actUser);
        }

        // GET: ActUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActUsers == null)
            {
                return NotFound();
            }

            var actUser = await _context.ActUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actUser == null)
            {
                return NotFound();
            }

            return View(actUser);
        }

        // POST: ActUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActUsers == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActUsers'  is null.");
            }
            var actUser = await _context.ActUsers.FindAsync(id);
            if (actUser != null)
            {
                _context.ActUsers.Remove(actUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActUserExists(int id)
        {
          return (_context.ActUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
