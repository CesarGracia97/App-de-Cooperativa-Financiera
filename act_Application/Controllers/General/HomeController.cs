using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using act_Application.Logic;
using act_Application.Models.BD;
using act_Application.Models.Sistema.ViewModels;
using act_Application.Helper;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using System.Security.Claims;
using System.Linq;
using act_Application.Data;
using static act_Application.Models.Sistema.Complementos.DetallesAportacionesUsers;

namespace act_Application.Controllers.General
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DesarrolloContext _context;

        public HomeController(ILogger<HomeController> logger, DesarrolloContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Menu()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                AportacionRepository aportacionRepository = new AportacionRepository();
                MultaRepository multaRepository = new MultaRepository();
                var multaUserList = multaRepository.GetDataMultasUser(userId);
                var aportacionesUserList = aportacionRepository.GetDataAportacionesUser(userId);
                var multaUser = multaUserList.FirstOrDefault();
                var aportacionesUser = aportacionesUserList.FirstOrDefault();

                EventosRepository eventosRepository = new EventosRepository();
                var eventosData = eventosRepository.GetDataEventos();
                List<Home_VM> viewModelList = new List<Home_VM>();

                // Verifica si la obtención de datos fue exitosa
                if (eventosRepository != null && aportacionesUserList.Count > 0)
                {
                    foreach (var evento in eventosData.Eventos)
                    {
                        Home_VM viewModel = new Home_VM
                        {
                            Eventos = evento,
                            Transacciones = _context.ActTransacciones.FirstOrDefault(t => t.Id == evento.IdTransaccion),
                            AportacionesUser = new Models.Sistema.Complementos.DetallesAportacionesUsers
                            {
                                AportacionesAcumuladas = aportacionesUser.AportacionesAcumuladas,
                                TotalAportaciones = aportacionesUser.TotalAportaciones,
                                TotalAprobados = aportacionesUser.TotalAprobados,
                                TotalEspera = aportacionesUser.TotalEspera
                            },
                            MultaUser = new Models.Sistema.Complementos.DetallesMultasUsers
                            {
                                MultasAcumuladas = multaUser.MultasAcumuladas,
                                TotalMultas = multaUser.TotalMultas,
                                TotalCancelados = multaUser.TotalCancelados

                            }
                        };

                        viewModelList.Add(viewModel);
                    }

                    // Pasa la lista de Home_VM a la vista
                    return View(viewModelList);
                }
            }

            return View(new List<Home_VM>()); // Devuelve una lista vacía si no se cumple la condición
        }
        public IActionResult Error()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> Participar(int IdP, [Bind("Id,IdTransaccion,Estado,FechaInicio,FechaFinalizacion,FechaGeneracion,ParticipantesId,ParticipantesNombre")] ActEvento actEvento)
        {
            actEvento.Id = IdP;
            if (IdP != actEvento.Id)
            {
                Console.WriteLine("Fallo la verificacion de Datos de la Edicion del Eventos");
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var eventoRepository = new EventosRepository();
                    var RparticipantesOriginal = eventoRepository.GetDataEventoPorId(IdP);

                    if (RparticipantesOriginal == null)
                    {
                        Console.WriteLine("Hubo un problema al momento de comprobar la nulidad de la Participacion");
                        return RedirectToAction("Error", "Home");
                    }

                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                        actEvento.IdUser = RparticipantesOriginal.IdUser;
                        actEvento.FechaInicio = RparticipantesOriginal.FechaInicio;
                        actEvento.FechaFinalizacion = RparticipantesOriginal.FechaFinalizacion;
                        actEvento.FechaGeneracion = RparticipantesOriginal.FechaGeneracion;
                        actEvento.ParticipantesId = userId.ToString() + ", ";
                        actEvento.ParticipantesNombre = userIdentificacion + ", ";
                        actEvento.IdTransaccion = RparticipantesOriginal.IdTransaccion;
                        actEvento.Estado = RparticipantesOriginal.Estado;

                        _context.Update(actEvento);
                        await _context.SaveChangesAsync();
                    }


                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al actualizar el registro en la participacion del Evento.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
                return RedirectToAction("Menu", "Home");
            }
            return View(actEvento);

        }
    }
}