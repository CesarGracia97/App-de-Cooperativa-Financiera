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
        public async Task<IActionResult> Edit(int Id, DateTime FechaPagoTotalPrestamo, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione) //Metodo para Transaccion Evaluada y confirmada.
        {
            if (Id != actTransaccione.Id)
            {
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                string razon, Descripcion, DescripcionC="";

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

                    razon = "RESPUESTA DE PRESTAMO ADMIN";
                    if (actTransaccione.TipoCuota == "PAGO UNICO")
                    {
                        DescripcionC = "El Administrador A Evaluado tu Peticion de Prestramo con La Condidion de la Fecha de Pago  Total para el dia " + actTransaccione.FechaPagoTotalPrestamo.ToString("dd-MMM-yyyy");

                    }
                    else if (actTransaccione.TipoCuota == "PAGO MENSUAL")
                    {
                        // Crea las cuotas y obtén las fechas de pago
                        List<DateTime> fechasDePago = await CrearCuotas(Id, FechaPagoTotalPrestamo, new ActCuota());

                        // Construye la descripción que incluye las fechas de pago
                        DescripcionC = "El Administrador A Evaluado tu Petición de Préstamo con La Condición de que las fechas de pago sean las siguientes:";
                        foreach (var fecha in fechasDePago)
                        {
                            DescripcionC += " " + fecha.ToString("dd-MMM-yyyy");
                        }
                    }
                    Descripcion = DescripcionC;

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

                await CrearNotificacion(actTransaccione.Id, 0, 0, actTransaccione.IdUser.ToString(), razon, Descripcion,  new ActNotificacione());
                return RedirectToAction("Menu", "Home"); // Puedes redirigir a donde desees después de la edición exitosa
            }
            return View(actTransaccione);
        }

        private async Task CrearNotificacion(int idTransaccion,int IdCuota, int IdAportacion, string Destino, string Razon, string Descripcion, [Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino,IdTransacciones,IdAportaciones,IdCuotas")] ActNotificacione actNotificacione) //Metodo para crear una nueva Notificacion en BD y notificacion al Usuario Remitente
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                actNotificacione.IdUser = userId;
                actNotificacione.Razon = Razon;
                actNotificacione.Descripcion = Descripcion;
                actNotificacione.FechaNotificacion = DateTime.Now;
                actNotificacione.Destino = Destino;
                if(idTransaccion > 0)  {
                    actNotificacione.IdTransacciones = idTransaccion;
                }
                else
                {
                    actNotificacione.IdTransacciones = 0;
                }
                if(IdAportacion > 0){
                    actNotificacione.IdAportaciones = IdAportacion;
                }
                else
                {
                    actNotificacione.IdAportaciones = 0;
                }
                actNotificacione.IdCuotas = 0;
                if(IdCuota > 0){
                    actNotificacione.IdCuotas = IdCuota; ;
                }
                else{
                    actNotificacione.IdCuotas = 0;
                }

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
        public async Task<IActionResult> EditTran(int Id, string Descripcion, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione) //Metodo para Transaccion Evaluada y Denegada.
        {
            if (Id != actTransaccione.Id)
            {
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                string razon;
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

                    razon = "PETICION DE PRESTAMO DENEGADA";

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
                await CrearNotificacion(actTransaccione.Id, 0, 0, actTransaccione.IdUser.ToString(), razon, Descripcion, new ActNotificacione());
                return RedirectToAction("Menu", "Home");
            }
            return View(actTransaccione);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminReferenteOnly")]
        public async Task<IActionResult> AceptarReferente(int Id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione)
        {
            if (Id != actTransaccione.Id)
            {
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                string Razon, Descripcion;
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

                    Razon = "CONDICIONES ACEPTADAS"; 
                    Descripcion ="El Usuario de la Transaccion "+ actTransaccione.Id+" a aceptado las condiciones del Prestamo";


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
                await CrearNotificacion(actTransaccione.Id, 0, 0, "ADMINISTRADOR", Razon, Descripcion, new ActNotificacione());
                return RedirectToAction("Menu", "Home"); // Puedes redirigir a donde desees después de la edición exitosa
            }
            return View(actTransaccione);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminReferenteOnly")]
        public async Task<IActionResult> RechazarReferente(int Id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione)
        {
            if (Id != actTransaccione.Id)
            {
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                string Razon, Descripcion;
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
                    actTransaccione.Estado = "RECHAZADO";
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();

                    Razon = "CONDICIONES RECHAZADAS"; 
                    Descripcion = "El Usuario de la Transaccion " + actTransaccione.Id + " a rechazado las condiciones del Prestamo";


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
                await CrearNotificacion(actTransaccione.Id, 0, 0, "ADMINISTRADOR", Razon, Descripcion, new ActNotificacione());
                return RedirectToAction("Menu", "Home"); // Puedes redirigir a donde desees después de la edición exitosa
            }
            return View(actTransaccione);

        }


        private async Task<List<DateTime>> CrearCuotas(int IdTransaccion, DateTime FechaPagoTotalPrestamo, [Bind("Id,IdUser,IdTransaccion,ValorCuota,FechaCuota,Estado")] ActCuota actCuota)
        {
            var metodoNotificacion = new MetodoNotificaciones();
            var transaccionOriginal = metodoNotificacion.GetTransaccionPorId(IdTransaccion);

            if (transaccionOriginal == null)
            {
                Console.WriteLine("Hubo un error al verificar la existencia del registro en GetTransaccionPorId(IdTransaccion)");
            }
            int numeroDeCuotas = (FechaPagoTotalPrestamo.Year - transaccionOriginal.FechaIniCoutaPrestamo.Year) * 12 +
                FechaPagoTotalPrestamo.Month - transaccionOriginal.FechaIniCoutaPrestamo.Month + 1;

            
            decimal valorDeCuota = transaccionOriginal.Valor / numeroDeCuotas;

            var fechasDeCuotas = new List<DateTime>();

            for (int i = 0; i < numeroDeCuotas; i++) 
            {
                DateTime fechaDeCuota = transaccionOriginal.FechaIniCoutaPrestamo.AddMonths(i);
                fechasDeCuotas.Add(fechaDeCuota);

                var cuota = new ActCuota
                {
                    IdUser = transaccionOriginal.IdUser,
                    IdTransaccion = IdTransaccion,
                    ValorCuota = valorDeCuota,
                    FechaCuota = fechaDeCuota,
                    Estado = "PENDIENTE DE PAGO"
                };

                _context.Add(cuota);
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return fechasDeCuotas;
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