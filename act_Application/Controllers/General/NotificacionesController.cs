using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Authorization;
using act_Application.Logica;
using act_Application.Models.Sistema;

namespace act_Application.Controllers.General
{
    public class NotificacionesController : Controller
    {
        private readonly DesarrolloContext _context;

        public NotificacionesController(DesarrolloContext context)
        {
            _context = context;
        }

        // GET: Notificaciones
        [Authorize]
        public IActionResult Index()
        {
            var metodoNotificacion = new MetodoNotificaciones();
            var notificaciones = metodoNotificacion.GetNotificacionesAdministrador();

            var viewModelList = notificaciones.Select(notificacion => new NT_ViewModel
            {
                Notificaciones = notificacion,
                Transacciones = _context.ActTransacciones.FirstOrDefault(t => t.Id == notificacion.IdTransacciones)
            });

            return View(viewModelList);
        }

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
                    actTransaccione.Estado = "Pendiente Referente";
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActTransaccionesExists(actTransaccione.Id))
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


        private bool ActTransaccionesExists(int id)
        {
          return _context.ActTransacciones.Any(e => e.Id == id);
        }
    }
}
