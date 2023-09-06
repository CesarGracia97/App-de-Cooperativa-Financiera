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

        public int _idTransaccionGlobal;
        public int _idUserGlobal;
        public DateTime _fechaPagoTotal;
        public string _descripcionGlobal;

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
                    var viewModelList = notificaciones.Select(notificacion => new NT_ViewModel
                    {
                        Notificaciones = notificacion,
                        Transacciones = _context.ActTransacciones.FirstOrDefault(t => t.Id == notificacion.IdTransacciones)
                    });

                    return View(viewModelList);
                }

                else

                {
                    if (!User.HasClaim("Rol", "Administrador") && (User.HasClaim("Rol", "Socio") || User.HasClaim("Rol", "Referido")))
                    {
                        var metodoNotificacion = new MetodoNotificaciones();
                        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                        int Bandera = 0;

                        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                        {
                            Bandera = userId;
                        }

                        var notificaciones = metodoNotificacion.GetNotificacionesUsuario(Bandera);

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

                if (viewModel == null)
                {
                    viewModel = new NT_ViewModel();
                }
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
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione) //Metodo para Transaccion Evaluada y confirmada.
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

                    actTransaccione.Razon = transaccionOriginal.Razon;
                    actTransaccione.IdUser = transaccionOriginal.IdUser;
                    actTransaccione.Valor = transaccionOriginal.Valor;
                    actTransaccione.FechaEntregaDinero = transaccionOriginal.FechaEntregaDinero;
                    actTransaccione.FechaIniCoutaPrestamo = transaccionOriginal.FechaIniCoutaPrestamo;
                    actTransaccione.TipoCuota = transaccionOriginal.TipoCuota;
                    actTransaccione.Estado = "PENDIENTE REFERENTE";
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();

                    _idTransaccionGlobal = actTransaccione.Id;
                    _idUserGlobal = actTransaccione.IdUser;
                    _fechaPagoTotal = actTransaccione.FechaPagoTotalPrestamo;

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
                await CrearNotificacion(new ActNotificacione());
                return RedirectToAction("Menu", "Home"); // Puedes redirigir a donde desees después de la edición exitosa
            }
            return View(actTransaccione);
        }
        
        private async Task CrearNotificacion([Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino,IdTransacciones,IdAportaciones,IdCuotas")] ActNotificacione actNotificacione) //Metodo para crear una nueva Notificacion en BD y notificacion al Usuario Remitente
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                actNotificacione.IdUser = userId;
                actNotificacione.Razon = "RESPUESTA DE PRESTAMO ADMIN";
                actNotificacione.Descripcion = "El Administrador A Evaluado tu Peticion de Prestramo con La Condidion de la Fecha de Pago  Total para la fecha "+ _fechaPagoTotal.ToString("dd-MMM-yyyy");
                actNotificacione.FechaNotificacion = DateTime.Now;
                actNotificacione.Destino = _idUserGlobal.ToString();
                actNotificacione.IdTransacciones = _idTransaccionGlobal;
                actNotificacione.IdCuotas = 0;
                actNotificacione.IdAportaciones = 0;

                _context.Add(actNotificacione);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Manejar el caso en que no se pueda obtener el Id del usuario
                ModelState.AddModelError("", "Error al obtener el Id del usuario.");
                Console.WriteLine("Fallo el guardado");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Denegado (int Id, [Bind("IdUser,Razon,Descripcion,FechaNotificacion,Destino")] ActNotificacione actNotificacione)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        var metodoNotificacion = new MetodoNotificaciones();
                        var notificacionOriginal = metodoNotificacion.GetNotificacionesUserId(Id);

                        actNotificacione.IdUser = userId;
                        actNotificacione.Razon = "PETICION DE PRESTAMO DENEGADA";
                        actNotificacione.FechaNotificacion = DateTime.Now;
                        actNotificacione.Destino = notificacionOriginal.IdUser.ToString();
                        actNotificacione.Descripcion = _descripcionGlobal;
                        actNotificacione.IdTransacciones = Id;
                        actNotificacione.IdCuotas = 0;
                        actNotificacione.IdAportaciones = 0;

                        _context.Add(actNotificacione);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al obtener el Id del usuario.");
                        Console.WriteLine("Fallo el guardado");
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hubo un error en el metodo Denegado");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
            }
            return View(actNotificacione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditTran(int Id, int IdN, string Descripcion, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione) //Metodo para Transaccion Evaluada y Denegada.
        {
            if (Id != actTransaccione.Id)
            {
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var metodoNotificacion = new MetodoNotificaciones();
                    var transaccionOriginal = metodoNotificacion.GetTransaccionPorId(Id);

                    if (transaccionOriginal == null)
                    {
                        return RedirectToAction("Error", "Home");
                    }

                    actTransaccione.Razon = transaccionOriginal.Razon;
                    actTransaccione.IdUser = transaccionOriginal.IdUser;
                    actTransaccione.Valor = transaccionOriginal.Valor;
                    actTransaccione.FechaEntregaDinero = transaccionOriginal.FechaEntregaDinero;
                    actTransaccione.FechaIniCoutaPrestamo = transaccionOriginal.FechaIniCoutaPrestamo;
                    actTransaccione.FechaIniCoutaPrestamo = transaccionOriginal.FechaPagoTotalPrestamo;
                    actTransaccione.TipoCuota = transaccionOriginal.TipoCuota;
                    actTransaccione.Estado = "DENEGADO";
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();

                    _idTransaccionGlobal = actTransaccione.Id;
                    _idUserGlobal = actTransaccione.IdUser;
                    _fechaPagoTotal = actTransaccione.FechaPagoTotalPrestamo;
                    _descripcionGlobal = Descripcion;

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
                await Denegado(IdN,new ActNotificacione());
                return RedirectToAction("Menu", "Home");
            }
            return View(actTransaccione);
        }

        public async Task<IActionResult> AceptarReferente(int Id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione)
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

                    actTransaccione.Razon = transaccionOriginal.Razon;
                    actTransaccione.IdUser = transaccionOriginal.IdUser;
                    actTransaccione.Valor = transaccionOriginal.Valor;
                    actTransaccione.FechaEntregaDinero = transaccionOriginal.FechaEntregaDinero;
                    actTransaccione.FechaIniCoutaPrestamo = transaccionOriginal.FechaIniCoutaPrestamo;
                    actTransaccione.TipoCuota = transaccionOriginal.TipoCuota;
                    actTransaccione.Estado = "APROBADO";
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();

                    _idTransaccionGlobal = actTransaccione.Id;
                    _idUserGlobal = actTransaccione.IdUser;
                    _fechaPagoTotal = actTransaccione.FechaPagoTotalPrestamo;

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
                await CrearNotificacion(new ActNotificacione());
                return RedirectToAction("Menu", "Home"); // Puedes redirigir a donde desees después de la edición exitosa
            }
            return View(actTransaccione);

        }


        public bool ActTransaccionesExists(int id) //Verifica la existencia de una Transaccion en Especifico 
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