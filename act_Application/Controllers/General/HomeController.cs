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
            EventosRepository eventosRepository = new EventosRepository();
            var eventosData = eventosRepository.GetDataEventos();

            // Verifica si la obtención de datos fue exitosa
            if (eventosData.TotalEventos >= 0)
            {
                List<Home_VM> viewModelList = new List<Home_VM>();
                foreach (var evento in eventosData.Eventos)
                {
                    Home_VM viewModel = new Home_VM
                    {
                        Eventos = evento,  // Asigna los datos del evento a la propiedad correspondiente de Home_VM
                        Transacciones = _context.ActTransacciones.FirstOrDefault(t => t.Id == evento.IdTransaccion)
                    };

                    viewModelList.Add(viewModel);
                }

                // Pasa la lista de Home_VM a la vista
                return View(viewModelList);
            }
            else
            {

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
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActParticipantesExist(actEvento.Id))
                    {
                        return RedirectToAction("Error", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Menu", "Home");
            }
            return View(actEvento);

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