using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using act_Application.Models.BD;
using act_Application.Models.Sistema.ViewModels;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using System.Security.Claims;
using act_Application.Data;

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
                List<Home_VM> viewModelList = new List<Home_VM>();

                AportacionRepository aportacionRepository = new AportacionRepository();
                var aportacionesUserList = aportacionRepository.GetDataAportacionesUser(userId);
                var aportacionesUser = aportacionesUserList.FirstOrDefault();
                Models.Sistema.Complementos.DetallesAportacionesUsers aportacionesUserVM = null;

                if (aportacionRepository.GetExistApotacionesUser(userId) == true)
                {
                    aportacionesUserVM = new Models.Sistema.Complementos.DetallesAportacionesUsers
                    {
                        AportacionesAcumuladas = aportacionesUser.AportacionesAcumuladas,
                        TotalAportaciones = aportacionesUser.TotalAportaciones,
                        TotalAprobados = aportacionesUser.TotalAprobados,
                        TotalEspera = aportacionesUser.TotalEspera
                    };
                }

                MultaRepository multaRepository = new MultaRepository();
                var multaUserList = multaRepository.GetDataMultasUser(userId);
                var multaUser = multaUserList.FirstOrDefault();
                Models.Sistema.Complementos.DetallesMultasUsers multaUserVM = null;

                if (multaRepository.GetExistMultasUser(userId) == true)
                {
                    multaUserVM = new Models.Sistema.Complementos.DetallesMultasUsers
                    {
                        MultasAcumuladas = multaUser.MultasAcumuladas,
                        TotalMultas = multaUser.TotalMultas,
                        TotalCancelados = multaUser.TotalCancelados
                    };
                }

                TransaccionesRepository transaccionesRepository = new TransaccionesRepository();
                var transaccionesUserList = transaccionesRepository.GetDataTransaccionesUser(userId);
                var transaccionUser = transaccionesUserList.FirstOrDefault();
                Models.Sistema.Complementos.DetallesTransaccionesUsers transaccionesUserVM = null;

                if(transaccionesRepository.GetExistTransaccionesUser(userId) == true)
                {
                    transaccionesUserVM = new Models.Sistema.Complementos.DetallesTransaccionesUsers {
                        TotalTransacciones = transaccionUser.TotalTransacciones,
                        TotalAprobado = transaccionUser.TotalAprobado,
                        TotalCuotas = transaccionUser.TotalCuotas,
                        TotalPagoUnico = transaccionUser.TotalPagoUnico,
                        TotalRechazado = transaccionUser.TotalRechazado,
                        ValorTotalPrestado = transaccionUser.ValorTotalPrestado,
                        TotalPendiente = transaccionUser.TotalPendiente
                    };
                }

                EventosRepository eventosRepository = new EventosRepository();
                var eventosData = eventosRepository.GetDataEventos();

                if (eventosRepository.GetExistEventos() == true)
                {
                    foreach (var evento in eventosData.Eventos)
                    {
                        Home_VM viewModel = new Home_VM
                        {
                            Eventos = evento,
                            Transacciones = _context.ActTransacciones.FirstOrDefault(t => t.Id == evento.IdTransaccion)
                        };

                        viewModelList.Add(viewModel);
                    }
                }

                foreach (var viewModel in viewModelList)
                {
                    viewModel.AportacionesUser = aportacionesUserVM;
                    viewModel.MultaUser = multaUserVM;
                    viewModel.TransaccionesUser = transaccionesUserVM;
                }
                return View(viewModelList);
            }
            return View(new List<Home_VM>());
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