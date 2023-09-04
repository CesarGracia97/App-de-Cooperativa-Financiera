using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Authorization;
using act_Application.Logica;
using act_Application.Models.Sistema;
using act_Application.Helper;
using MySql.Data.MySqlClient;


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

        // POST: ActTransacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
