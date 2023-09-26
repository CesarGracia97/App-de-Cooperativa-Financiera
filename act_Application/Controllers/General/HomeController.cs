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
            try
            {
                var metodoeventos = new MetodoEventos();
                var eventos = metodoeventos.GetDataEventos();
                var viewModelList = eventos.Select(evento => new Home_VM
                {
                    Participante = evento
                });
                return View(viewModelList);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Se produjo un error al Cargar los Datos del ViewModel: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
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

        public async Task<IActionResult> Participar(int IdP, [Bind("Id,IdTransaccion,Estado,FechaInicio,FechaFinalizacion,FechaGeneracion,ParticipantesId,ParticipantesNombre")] ActParticipante actParticipante)
        {
            actParticipante.Id = IdP;
            if (IdP != actParticipante.Id)
            {
                Console.WriteLine("Fallo la verificacion de Datos de la Edicion del Participante");
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var metodoNotificacion = new MetodoNotificaciones();
                    var RparticipantesOriginal = metodoNotificacion.GetRegistroParticipante(IdP);

                    if (RparticipantesOriginal == null)
                    {
                        Console.WriteLine("Hubo un problema al momento de comprobar la nulidad de la Participacion");
                        return RedirectToAction("Error", "Home");
                    }

                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                        actParticipante.FechaInicio = RparticipantesOriginal.FechaInicio;
                        actParticipante.FechaFinalizacion = RparticipantesOriginal.FechaFinalizacion;
                        actParticipante.FechaGeneracion = RparticipantesOriginal.FechaGeneracion;
                        actParticipante.ParticipantesId = userId.ToString() + ", ";
                        actParticipante.ParticipantesNombre = userIdentificacion + ", ";
                        actParticipante.IdTransaccion = RparticipantesOriginal.IdTransaccion;
                        actParticipante.Estado = RparticipantesOriginal.Estado;

                        _context.Update(actParticipante);
                        await _context.SaveChangesAsync();
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActParticipantesExist(actParticipante.Id))
                    {
                        return RedirectToAction("Error", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(actParticipante);

        }


        public bool ActParticipantesExist(int id)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = ConfigReader.GetQuery("SelectParticipantesExist"); ;

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdTransaccion", id);
                        object result = command.ExecuteScalar();
                        int count = Convert.ToInt32(result);

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error al verificar la existencia del registro de la participacion.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return false;
            }
        }

    }
}