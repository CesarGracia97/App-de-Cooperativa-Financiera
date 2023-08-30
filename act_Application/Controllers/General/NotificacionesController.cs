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

            // Convierte la lista de ActNotificacione a una lista de NT_ViewModel
            var viewModelList = notificaciones.Select(notificacion => new NT_ViewModel
            {
                Notificaciones = notificacion,
                Transacciones = new ActTransaccione() // Puedes inicializar Transacciones si es necesario
            });
            return View(viewModelList);
        }

        private bool ActTransaccionesExists(int id)
        {
          return _context.ActTransacciones.Any(e => e.Id == id);
        }
    }
}
