using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Authorization;
using act_Application.Logica;
using act_Application.Models.Sistema;
using act_Application.Helper;
using MySql.Data.MySqlClient;
using System.Security.Claims;


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
        [Authorize (Policy = "AllOnly")]
        public IActionResult Index()
        {
            try
            {
                NT_ViewModel viewModel = null;

                if (User.HasClaim("Rol", "Administrador"))
                {
                    var metodoNotificacion = new MetodoNotificaciones();
                    var notificaciones = metodoNotificacion.GetNotificacionesAdministrador();

                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {

                    }

                    return View();
                }
                else
                {
                    if (!User.HasClaim("Rol", "Administrador") && (User.HasClaim("Rol", "Socio") || User.HasClaim("Rol", "Referido")))
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
                    else
                    {
                        Console.WriteLine("Este usuario no posee un rol.");
                    }
                }

                viewModel ??= new NT_ViewModel();

                return View(viewModel);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se produjo un error en el rol del Usuario: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione)
        {
            if (Id != actTransaccione.Id)
            {
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var metodoNotificacion = new MetodoNotificaciones(); // Crear una instancia de MetodoNotificaciones
                    var transaccionOriginal = metodoNotificacion.GetTransaccionPorId(Id); // Llamar al método desde la instancia

                    if (transaccionOriginal == null)
                    {
                        return RedirectToAction("Error", "Home");
                    }

                    // Asignar los valores originales a la transacción que se va a editar
                    actTransaccione.Razon = transaccionOriginal.Razon;
                    actTransaccione.IdUser = transaccionOriginal.IdUser;
                    actTransaccione.Valor = transaccionOriginal.Valor;
                    actTransaccione.FechaEntregaDinero = transaccionOriginal.FechaEntregaDinero;
                    actTransaccione.FechaIniCoutaPrestamo = transaccionOriginal.FechaIniCoutaPrestamo;
                    actTransaccione.TipoCuota = transaccionOriginal.TipoCuota;
                    actTransaccione.Estado = "Pendiente Referente";
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActTransaccionesExists(actTransaccione.Id))
                    {
                        return RedirectToAction("Error", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Menu", "Home"); // Puedes redirigir a donde desees después de la edición exitosa
            }
            return View(actTransaccione);
        }

        public bool ActTransaccionesExists(int id)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = ConfigReader.GetQuery("SelectExistTransancion"); ;

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        object result = command.ExecuteScalar();
                        int count = Convert.ToInt32(result);

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error al verificar la existencia del registro.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return false;
            }
        }
    }
}