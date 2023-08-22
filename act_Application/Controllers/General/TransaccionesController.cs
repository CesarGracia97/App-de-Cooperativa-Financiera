using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;

namespace act_Application.Controllers.General
{
    public class TransaccionesController : Controller
    {
        private readonly DesarrolloContext _context;

        public TransaccionesController(DesarrolloContext context)
        {
            _context = context;
        }



        // GET: Transacciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Razon,IdUser,Valor,Estado,FechPagoTotalPrestamo,FechaIniCoutaPrestamo")] ActTransaccione actTransaccione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actTransaccione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actTransaccione);
        }

        private bool ActTransaccioneExists(int id)
        {
          return _context.ActTransacciones.Any(e => e.Id == id);
        }
    }
}
