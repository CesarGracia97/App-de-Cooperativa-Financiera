using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;
using act_Application.Models.Sistema;

namespace act_Application.Controllers.General
{
    public class TransaccionesController : Controller
    {
        private readonly DesarrolloContext _context;

        public TransaccionesController(DesarrolloContext context)
        {
            _context = context;
        }

        private List<ListItems> ObtenerItemsRazon()           //Contenido de la Lista Razones
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "PRESTAMO" }
            };
        }

        private List<ListItems> ObtenerItemsCuota()           //Contenido de la Lista Cuotas
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "PAGO UNICO" },
                new ListItems { Id = 2, Nombre = "PAGO MENSUAL" }
            };
        }

        // GET: Aportar/Create
        public IActionResult Create()
        {

            ViewData["ItemsRazon"] = ObtenerItemsRazon();
            ViewData["ItemsCuota"] = ObtenerItemsCuota();

            return View();
        }

        // POST: Transacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Razon,IdUser,Valor,Estado,FechPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione)
        {
            if (ModelState.IsValid)
            {
                if (actTransaccione.TipoCuota == "PAGO UNICO")
                {
                    actTransaccione.FechPagoTotalPrestamo = actTransaccione.FechaIniCoutaPrestamo.AddDays(31);
                }
                else if (actTransaccione.TipoCuota == "PAGO MENSUAL")
                {
                    actTransaccione.FechPagoTotalPrestamo = actTransaccione.FechaIniCoutaPrestamo.AddDays(90);
                }
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
