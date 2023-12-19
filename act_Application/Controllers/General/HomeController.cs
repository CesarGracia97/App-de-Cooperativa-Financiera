using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Models;
using act_Application.Models.BD;
using act_Application.Services.ServiciosAplicativos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using act_Application.Models.Sistema.ViewModel;

namespace act_Application.Controllers.General
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NotificacionesServices _nservices;
        private readonly ActDesarrolloContext _context;
        public HomeController(ILogger<HomeController> logger, ActDesarrolloContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize(Policy = "AllOnly")]
        public IActionResult Index()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                List<Home_VM> viewModelList = new List<Home_VM>();
                if((bool) new EventosRepository().OperacionesEventos(1, 0, 0, ""))
                {
                    var eobj = (List<ActEvento>)new EventosRepository().OperacionesEventos(2, 0, 0, "");
                    foreach (var eventos in eobj)
                    {
                        Home_VM viewModel = new Home_VM
                        {
                            Eventos = eventos,
                            Prestamos = _context.ActPrestamos.FirstOrDefault(t => t.Id == eventos.IdPrestamo)
                        };
                        viewModelList.Add(viewModel);
                    }
                    return View(viewModelList);
                }
            }
            return View(new List<Home_VM>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Participar(int Id, [Bind("Id,IdEven,IdPrestamo,IdUser,ParticiantesId,NombresPId,FechaGeneracion,FechaInicio,FechaFinalizacion,Estado")] ActEvento actEvento)
        {
            if(Id != actEvento.Id)
            {
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var eobj = (ActEvento) new EventosRepository().OperacionesEventos( 3, Id, 0, "");

                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                    {
                        //
                        var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        //
                        actEvento.IdEven = eobj.IdEven;
                        actEvento.IdPrestamo = eobj.IdPrestamo;
                        actEvento.IdUser = actEvento.IdUser;
                        actEvento.ParticipantesId = eobj.ParticipantesId + IdUser.ToString() + ",";
                        actEvento.NombresPId = eobj.NombresPId + userIdentificacion + ",";
                        actEvento.FechaGeneracion = eobj.FechaGeneracion;
                        actEvento.FechaInicio = eobj.FechaInicio;
                        actEvento.FechaFinalizacion = eobj.FechaFinalizacion;
                        actEvento.Estado = eobj.Estado;
                        string Razon = $"Un Usuario a decidio Participar";
                        string DescripcionU = $"El Usuario a {userIdentificacion} a decidido participar como tu Garante en el evento de participacion de Garantes de tu Solicitud de Prestamo ID: {actEvento.IdPrestamo}." +
                                            $"\nSolcitud Realizada el dia {DateTime.Now}";
                        await _nservices.CrearNotificacion(2, 6, 0, IdUser, actEvento.IdEven, Razon, DescripcionU, actEvento.IdUser.ToString(), new ActNotificacione());
                        var essU = new EmailSendServices().EnviarCorreoUsuario(actEvento.IdUser, 8, DescripcionU); //IdUser apuntado al usuario dueño del evento del Prestamo para que este enterado.
                    }
                    _context.Update(actEvento);
                    await _context.SaveChangesAsync();


                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"\n----------------------------------------");
                    Console.WriteLine($"\nParticipar()-HomeController | Error.");
                    Console.WriteLine($"\nDetalles del error: " + ex.Message);
                    Console.WriteLine($"\n----------------------------------------");
                }
            }
            return View(actEvento);
        }
        public ActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }
    }
}