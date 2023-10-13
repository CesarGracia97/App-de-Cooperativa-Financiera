﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Authorization;
using act_Application.Logic;
using act_Application.Helper;
using MySql.Data.MySqlClient;
using act_Application.Models.Sistema.ViewModels;

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
        public IActionResult Notificacion()
        {
            try
            {
                Notificaciones_VM viewModel = null;

                if (User.HasClaim("Rol", "Administrador"))
                {
                    var notificacionesRepository = new NotificacionesRepository();
                    var notificaciones = notificacionesRepository.GetDataNotificacionesAdmin();
                    var viewModelList = notificaciones.Select(notificacion => new Notificaciones_VM
                    {
                        Notificaciones = notificacion,
                        Transacciones = _context.ActTransacciones.FirstOrDefault(t => t.Id == notificacion.IdTransacciones),
                        Eventos = _context.ActEventos.FirstOrDefault(t => t.Id == notificacion.IdTransacciones)
                    });

                    return View(viewModelList);
                }

                else

                {
                    if (!User.HasClaim("Rol", "Administrador") && (User.HasClaim("Rol", "Socio") || User.HasClaim("Rol", "Referido")))
                    {
                        var notificacionesRepository = new NotificacionesRepository();
                        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                        int Bandera = 0;

                        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                        {
                            Bandera = userId;
                        }

                        var notificaciones = notificacionesRepository.GetDataNotificacionesUser(Bandera);

                        var viewModelList = notificaciones.Select(notificacion => new Notificaciones_VM
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
                    viewModel = new Notificaciones_VM();
                }
                return View(viewModel);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Se produjo un error en el rol del Usuario: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        
        [HttpPost][ValidateAntiForgeryToken][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditTConfirmado (int Id, DateTime FechaPagoTotalPrestamo, DateTime FechaInicio, DateTime FechaFinalizacion, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota,IdParticipantes,FechaGeneracion")] ActTransaccione actTransaccione) //Metodo para Transaccion Evaluada y confirmada.
        {
            if (Id != actTransaccione.Id)
            {
                Console.WriteLine("Hubo un problema al momento de comprobar la validacion de la Transaccion en el Confirmado");
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                string razon="", Descripcion="", DescripcionC="";

                try
                {
                    var metodoNotificacion = new TransaccionesRepository(); // Crear una instancia de MetodoNotificaciones
                    var transaccionOriginal = metodoNotificacion.GetDataTransaccionId(Id); // Llamar al método desde la instancia
                    if (transaccionOriginal == null)
                    {
                        Console.WriteLine("Hubo un problema al momento de comprobar la nulidad de la Transaccion en el Confirmado");
                        return RedirectToAction("Error", "Home");
                    }

                    actTransaccione.Razon = transaccionOriginal.Razon;
                    actTransaccione.IdUser = transaccionOriginal.IdUser;
                    actTransaccione.Valor = transaccionOriginal.Valor;
                    actTransaccione.FechaPagoTotalPrestamo = FechaPagoTotalPrestamo;
                    actTransaccione.FechaEntregaDinero = transaccionOriginal.FechaEntregaDinero;
                    actTransaccione.FechaIniCoutaPrestamo = transaccionOriginal.FechaIniCoutaPrestamo;
                    actTransaccione.TipoCuota = transaccionOriginal.TipoCuota;
                    actTransaccione.Estado = "PENDIENTE REFERENTE";
                    actTransaccione.FechaGeneracion = transaccionOriginal.FechaGeneracion;
                    int idParticipantes = await CrearParticipaciones(Id, transaccionOriginal.IdUser,FechaInicio, FechaFinalizacion, new ActEvento());
                    if (idParticipantes != -1)
                    {
                        actTransaccione.IdParticipantes = idParticipantes;
                    }
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
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al actualizar el registro de la transaccion confirmada.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }

                await CrearNotificacion(actTransaccione.Id, 0, 0, actTransaccione.IdUser.ToString(), razon, Descripcion,  new ActNotificacione());
                return RedirectToAction("Menu", "Home"); // Puedes redirigir a donde desees después de la edición exitosa
            }
            return View(actTransaccione);
        }


        private async Task CrearNotificacion(int idTransaccion, int IdCuota, int IdAportacion, string Destino, string Razon, string Descripcion, [Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino,IdTransacciones,IdAportaciones,IdCuotas")] ActNotificacione actNotificacione) //Metodo para crear una nueva Notificacion en BD y notificacion al Usuario Remitente
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


        [HttpPost][ValidateAntiForgeryToken][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditTDenegado(int Id, string Descripcion, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota,IdParticipantes,FechaGeneracion")] ActTransaccione actTransaccione) //Metodo para Transaccion Evaluada y Denegada.
        {
            if (Id != actTransaccione.Id)
            {
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                string razon = "";
                try
                {
                    var metodoNotificacion = new TransaccionesRepository ();
                    var transaccionOriginal = metodoNotificacion.GetDataTransaccionId(Id);

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
                    actTransaccione.IdParticipantes = transaccionOriginal.IdParticipantes;
                    actTransaccione.FechaGeneracion = transaccionOriginal.FechaGeneracion;
                    actTransaccione.TipoCuota = transaccionOriginal.TipoCuota;
                    actTransaccione.Estado = "DENEGADO";

                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();

                    razon = "PETICION DE PRESTAMO DENEGADA";

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al actualizar el registro de la transaccion Denegada por el Admin.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
                await CrearNotificacion(actTransaccione.Id, 0, 0, actTransaccione.IdUser.ToString(), razon, Descripcion, new ActNotificacione());
                return RedirectToAction("Menu", "Home");
            }
            return View(actTransaccione);
        }

        
        [HttpPost][ValidateAntiForgeryToken][Authorize(Policy = "AdminReferenteOnly")]
        public async Task<IActionResult> AceptarReferente (int Id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota,IdParticipantes,FechaGeneracion,")] ActTransaccione actTransaccione)
        {
            if (Id != actTransaccione.Id)
            {
                Console.WriteLine("Hubo un problema al momento de comprobar la validacion de la Transaccion en la Aceptacion");
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                string Razon = "", Descripcion = "";
                try
                {
                    var metodoNotificacion = new TransaccionesRepository(); // Crear una instancia de MetodoNotificaciones
                    var transaccionOriginal = metodoNotificacion.GetDataTransaccionId(Id); // Llamar al método desde la instancia

                    if (transaccionOriginal == null)
                    {
                        Console.WriteLine("Hubo un problema al momento de comprobar la nulidad de la Transaccion en el Rechazo");
                        return RedirectToAction("Error", "Home");
                    }

                    actTransaccione.Razon = transaccionOriginal.Razon;
                    actTransaccione.IdUser = transaccionOriginal.IdUser;
                    actTransaccione.Valor = transaccionOriginal.Valor;
                    actTransaccione.FechaEntregaDinero = transaccionOriginal.FechaEntregaDinero;
                    actTransaccione.FechaIniCoutaPrestamo = transaccionOriginal.FechaIniCoutaPrestamo;
                    actTransaccione.FechaPagoTotalPrestamo = transaccionOriginal.FechaPagoTotalPrestamo;
                    actTransaccione.TipoCuota = transaccionOriginal.TipoCuota;
                    actTransaccione.FechaGeneracion = transaccionOriginal.FechaGeneracion;
                    actTransaccione.IdParticipantes = transaccionOriginal.IdParticipantes;
                    actTransaccione.Estado = "APROBADO";
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();

                    Razon = "CONDICIONES ACEPTADAS"; 
                    Descripcion ="El Usuario de la Transaccion "+ actTransaccione.Id+" a aceptado las condiciones del Prestamo";


                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al actualizar el registro en la participacion del Evento.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
                await EditParticipacion(actTransaccione.IdParticipantes, "CONCURSO", new ActEvento());
                await CrearNotificacion(actTransaccione.Id, 0, 0, "ADMINISTRADOR", Razon, Descripcion, new ActNotificacione());
                return RedirectToAction("Menu", "Home");// Puedes redirigir a donde desees después de la edición exitosa
            }
            return View(actTransaccione);

        }


        [HttpPost][ValidateAntiForgeryToken][Authorize(Policy = "AdminReferenteOnly")]
        public async Task <IActionResult> RechazarReferente (int Id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,TipoCuota,IdParticipantes,FechaGeneracion")] ActTransaccione actTransaccione)
        {
            if (Id != actTransaccione.Id)
            {
                Console.WriteLine("Hubo un problema al momento de comprobar la validacion de la Transaccion en el Rechazo del Referente");
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                string Razon ="", Descripcion ="";
                try
                {
                    var metodoTransRepo = new TransaccionesRepository(); // Crear una instancia de MetodoNotificaciones
                    var transaccionOriginal = metodoTransRepo.GetDataTransaccionId(Id); // Llamar al método desde la instancia

                    if (transaccionOriginal == null)
                    {
                        Console.WriteLine("Hubo un problema al momento de comprobar la nulidad de la Transaccion en el Rechazo del Referente");
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
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al actualizar el registro de la transaccion Rechazada por el Referente.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
                await EditParticipacion(actTransaccione.IdParticipantes, "DENEGADO", new ActEvento());
                await CrearNotificacion(actTransaccione.Id, 0, 0, "ADMINISTRADOR", Razon, Descripcion, new ActNotificacione());
                return RedirectToAction("Menu", "Home");
            }
            return View(actTransaccione);

        }


        private async Task<List<DateTime>> CrearCuotas(int IdTransaccion, DateTime FechaPagoTotalPrestamo, [Bind("Id,IdUser,IdTransaccion,ValorCuota,FechaCuota,Estado")] ActCuota actCuota)
        {
            var metodoNotificacion = new TransaccionesRepository();
            var transaccionOriginal = metodoNotificacion.GetDataTransaccionId(IdTransaccion);

            if (transaccionOriginal == null)
            {
                Console.WriteLine("Hubo un error al verificar la existencia del registro en GetDataTransaccionId(IdTransaccion)");
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

        private async Task<int> CrearParticipaciones(int IdTransaccion, int IdUser, DateTime FechaInicio, DateTime FechaFinalizacion, [Bind("Id,IdTransaccion,Estado,FechaInicio,FechaFinalizacion,FechaGeneracion,ParticipantesId,ParticipantesNombre,IdUser")] ActEvento actParticipante)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    actParticipante.FechaInicio = FechaInicio;
                    actParticipante.FechaFinalizacion = FechaFinalizacion;
                    actParticipante.FechaGeneracion = DateTime.Now;
                    actParticipante.ParticipantesId = "";
                    actParticipante.ParticipantesNombre = "";
                    actParticipante.IdTransaccion = IdTransaccion;
                    actParticipante.IdUser = IdUser;
                    actParticipante.Estado = "PENDIENTE";
                    /*ESTADOS:  PENDIENTE (ESESPERA DE LA RESPUESTA DEL USUARIO NO ADMIN).
                     *          EN CREACION (A LA ESPERA KUE EL ADMIN CONFIGURE LOS LIMITES ANTES DE LA PUBLICACION)
                                CONCURSO (A LA ESPERA DE KUE NUEVOS SOCIOS SE UNAN).
                                DENEGADO (EL USUARIO NO ADMIN NO PERMITIO EL SEGUIMIENTO DE LA TRANSACCION).
                                FINALIZADO (EL CONCURSO TERMINO Y SE SELECCIONARON LOS PARTICIPANTES.*/
                    _context.Add(actParticipante);
                    await _context.SaveChangesAsync();
                }
                return actParticipante.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un problema al crear el Registro de los participantes.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return -1;
            }

        }


        private async Task<IActionResult> EditParticipacion(int Id, string Estado, [Bind("Id,IdTransaccion,Estado,FechaInicio,FechaFinalizacion,FechaGeneracion,ParticipantesId,ParticipantesNombre,IdUser")] ActEvento actParticipante)
        {
            actParticipante.Id = Id;
            if (Id != actParticipante.Id)
            {
                Console.WriteLine("Fallo la verificacion de Datos de la Edicion del Eventos");
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var eventoRepository = new EventosRepository();
                    var RparticipantesOriginal = eventoRepository.GetDataEventoPorId(Id);

                    if (RparticipantesOriginal == null)
                    {
                        Console.WriteLine("Hubo un problema al momento de comprobar la nulidad de la Participacion e");
                        return RedirectToAction("Error", "Home");
                    }

                    actParticipante.FechaInicio = RparticipantesOriginal.FechaInicio;
                    actParticipante.FechaFinalizacion = RparticipantesOriginal.FechaFinalizacion;
                    actParticipante.FechaGeneracion = RparticipantesOriginal.FechaGeneracion;
                    actParticipante.ParticipantesId = RparticipantesOriginal.ParticipantesId;
                    actParticipante.ParticipantesNombre = RparticipantesOriginal.ParticipantesNombre;
                    actParticipante.IdTransaccion = RparticipantesOriginal.IdTransaccion;
                    actParticipante.Estado = Estado;
                    actParticipante.IdUser = RparticipantesOriginal.IdUser;


                    _context.Update(actParticipante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al actualizar el registro en la participacion del Evento.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
            }
            return View(actParticipante);
        }
    }
}