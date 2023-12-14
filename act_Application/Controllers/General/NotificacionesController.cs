﻿using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using act_Application.Models.Sistema.ViewModel;
using act_Application.Services.ServiciosAplicativos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace act_Application.Controllers.General
{
    public class NotificacionesController : Controller
    {
        private readonly ActDesarrolloContext _context;

        public NotificacionesController(ActDesarrolloContext context)
        {
            _context = context;
        }
        private List<ListItems> ItemsTipoUsuario()
        {
            return new List<ListItems>
            {
                new ListItems{Id = 1, Nombre = "Socio"},
                new ListItems{Id = 2, Nombre = "Referido"},
                new ListItems{Id = 3, Nombre = "En Espera"}
            };
        }
        private List<ListItems> ItemsTipoEstado()
        {
            return new List<ListItems>
            {
                new ListItems{Id = 1, Nombre = "ACTIVO"},
                new ListItems{Id = 2, Nombre = "INACTIVO"},
                new ListItems{Id = 3, Nombre = "EN EVALUACION"},
                new ListItems{Id = 4, Nombre = "DENEGADO"}

            };
        }
        [Authorize(Policy = "AllOnly")]
        public IActionResult Index()
        {
            Notificaciones_VM viewModel = null;
            try
            {
                if (User.HasClaim("Rol", "Administrador"))
                {
                    ViewData["ItemEstado"] = ItemsTipoEstado();
                    ViewData["ItemTipoUs"] = ItemsTipoUsuario();
                    var notiAdmi = (List<ActNotificacione>)new NotificacionesRepository().OperacionesNotificaciones(3, 0, 0);
                    var viewModelList = notiAdmi.Select(notificacion => new Notificaciones_VM
                    {
                        Notificaciones = notificacion,
                        Prestamos = _context.ActPrestamos.FirstOrDefault(t => t.IdPres == notificacion.IdActividad),
                        Cuotas = _context.ActCuotas.FirstOrDefault(t => t.IdCuot == notificacion.IdActividad),
                        Aportaciones = _context.ActAportaciones.FirstOrDefault(t => t.IdApor == notificacion.IdActividad),
                        Eventos = _context.ActEventos.FirstOrDefault(t => t.IdEven == notificacion.IdActividad),
                        Multas = _context.ActMultas.FirstOrDefault(t => t.IdMult == notificacion.IdActividad),
                        Usuarios = _context.ActUsers.FirstOrDefault(t => t.Cedula == notificacion.IdActividad)
                    }).ToList();
                    return View(viewModelList);
                }
                else
                {
                    if (!User.HasClaim("Rol", "Administrador") && (User.HasClaim("Rol", "Socio") || User.HasClaim("Rol", "Referido")))
                    {
                        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                        int Bandera = 0;
                        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                            Bandera = IdUser;
                        var notiUser = (List<ActNotificacione>)new NotificacionesRepository().OperacionesNotificaciones(4, 0, Bandera);
                        var viewModelList = notiUser.Select(notificacion => new Notificaciones_VM
                        {
                            Notificaciones = notificacion,
                            Prestamos = _context.ActPrestamos.FirstOrDefault(t => t.IdPres == notificacion.IdActividad),
                            Cuotas = _context.ActCuotas.FirstOrDefault(t => t.IdCuot == notificacion.IdActividad),
                            Eventos = _context.ActEventos.FirstOrDefault(t => t.IdEven == notificacion.IdActividad),
                            Multas = _context.ActMultas.FirstOrDefault(t => t.IdMult == notificacion.IdActividad),
                        });
                        return View(viewModelList);
                    }
                    else
                    {
                        Console.WriteLine("\n--------------------------------------------------------------------");
                        Console.WriteLine("\nError.");
                        Console.WriteLine("\nNotificacionesController-Index() | Este usuario no posee un rol.");
                        Console.WriteLine("\n--------------------------------------------------------------------\n");
                    }
                    if (viewModel == null)
                    {
                        viewModel = new Notificaciones_VM();
                    }
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n--------------------------------------------------------------------");
                Console.WriteLine($"\nError.");
                Console.WriteLine($"\nNotificacionesController-Index() | {ex.Message} ");
                Console.WriteLine($"\n--------------------------------------------------------------------\n");
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> Visualizado(int IdN, int IdA, int Opcion, string Estado, string TipoUser, DateTime FechaPagoTotalPrestamo, DateTime FechaInicioPagoCuotas, string TipoCuota, string PEstado, [Bind("Id,IdActividad,FechaGeneracion,Razon,Descripcion,Destino,Visto")] ActNotificacione actNotificacione)
        {
            actNotificacione.Id = IdN;
            if (IdN != actNotificacione.Id)
            {
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if(User.HasClaim("Rol", "Administrador")){
                        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                        {
                            var nobj = (ActNotificacione)new NotificacionesRepository().OperacionesNotificaciones(5, IdN, 0);
                            actNotificacione.IdUser = nobj.IdUser;
                            actNotificacione.IdActividad = nobj.IdActividad;
                            actNotificacione.FechaGeneracion = nobj.FechaGeneracion;
                            actNotificacione.Razon = nobj.Razon;
                            actNotificacione.Descripcion = nobj.Descripcion;
                            actNotificacione.Destino = nobj.Destino;
                            actNotificacione.Visto = "SI";
                            _context.Update(actNotificacione);
                            await _context.SaveChangesAsync();
                            switch (Opcion)
                            {
                                case 1/*Confirmar o Rechazar Nuevo Usuario*/:
                                    await Update_EstadoUsuario(IdA, IdUser,actNotificacione.IdActividad, Estado, TipoUser, new ActUser());
                                    return RedirectToAction("Index", "Notificaciones");
                                case 2/*Solicitud de Prestamo*/:
                                    await Update_EstadoPrestamo(IdA, IdUser, actNotificacione.IdActividad, FechaPagoTotalPrestamo, FechaInicioPagoCuotas, TipoCuota, PEstado, new ActPrestamo());
                                    return RedirectToAction("Index", "Notificaciones");
                                default:
                                    break;
                            }
                        }
                    }
                    else if (!User.HasClaim("Rol", "Administrador") && User.HasClaim("Rol", "Socio") || User.HasClaim("Rol", "Referido"))
                    {
                        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                        {

                        }
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Hubo un problema al actualizar 'Visto' en el Id.{IdN}");
                    Console.WriteLine($"Detalles del error: {ex.Message}");
                    return RedirectToAction("Error", "Home");
                }
            }
            return View(actNotificacione);
        }
        private async Task<IActionResult> Update_EstadoUsuario(int Id, int IdUser, string IdActividad, string Estado, string TipoUser, [Bind("Id,Cedula,NombreYApellido,Contrasena,Celular,TipoUser,IdSocio,FotoPerfil")] ActUser actUser)
        {
            string Razon = string.Empty, Descripcion = string.Empty;
            actUser.Id = Id;
            if (Id != actUser.Id)
            {
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var uobj = (ActUser)new UsuarioRepository().OperacionesUsuario(6, Id, 0, "", "");
                    actUser.Cedula = uobj.Cedula;
                    actUser.Correo = uobj.Correo;
                    actUser.NombreYapellido = uobj.NombreYapellido;
                    actUser.Celular = uobj.Celular;
                    actUser.Contrasena = uobj.Contrasena;
                    actUser.IdSocio = uobj.IdSocio;
                    actUser.FotoPerfil = uobj.FotoPerfil;
                    if (Estado == "ACTIVO")
                    {
                        actUser.Estado = Estado;
                        actUser.TipoUser = TipoUser;
                        Razon = $"CUENTA ACTIVADA";
                        Descripcion = $"Tu cuenta a sido Activada Exitosamente. Se te dio la categoria de {TipoUser}. Puedes acceder al sistema con tus credenciales.";

                    }
                    else
                    if (Estado != "ACTIVO" && Estado == "INACTIVO" || Estado == "DENEGADO" || Estado == "EN EVALUACION")
                    {
                        actUser.Estado = Estado;
                        actUser.TipoUser = "Denegado";
                        Descripcion = $"Tu peticion de ingreso fue denegada...";

                    }
                    await new NotificacionesServices(_context).CrearNotificacion(4, IdUser, IdActividad, Razon, Descripcion, actUser.Id.ToString(), new ActNotificacione());
                    await new EmailSendServices().EnviarCorreoUsuario(actUser.Id, 11,Descripcion);
                    _context.Update(actUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Hubo un problema al actualizar el registro del Estado del Usuario en el Id.{Id}");
                    Console.WriteLine($"Detalles del error: {ex.Message}");
                    return RedirectToAction("Error", "Home");
                }
            }
            return View(actUser);
        }
        private async Task<IActionResult> Update_EstadoPrestamo(int Id, int IdUser, string IdActividad, DateTime FechaPagoTotalPrestamo, DateTime FechaInicioPagoCuotas, string TipoCuota, string PEstado, [Bind("Id,IdPres,IdUser,Valor,FechaGeneracion,FechaEntregaDinero,FechaInicioPagoCuotas,FechaPagoTotalPrestamo,TipoCuota,Estado")] ActPrestamo actPrestamo)
        {
            string Razon = string.Empty, Descripcion = string.Empty;
            actPrestamo.Id = Id;
            if (Id != actPrestamo.Id)
            {
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (PEstado == "ACEPTADO")
                        {
                            var pobj = (ActPrestamo)new PrestamosRepository().OperacionesPrestamos(2, Id, 0, "");
                            actPrestamo.IdPres = pobj.IdPres;
                            actPrestamo.IdUser = pobj.IdUser;
                            actPrestamo.Valor = pobj.Valor;
                            actPrestamo.FechaGeneracion = pobj.FechaGeneracion;
                            actPrestamo.FechaEntregaDinero = pobj.FechaEntregaDinero;
                            actPrestamo.FechaPagoTotalPrestamo = FechaPagoTotalPrestamo;
                            actPrestamo.FechaInicioPagoCuotas = FechaInicioPagoCuotas;
                            actPrestamo.TipoCuota = TipoCuota;
                            actPrestamo.Estado = PEstado;
                            _context.Update(actPrestamo);
                            Razon = $"Solicitud de Prestamos Aceptado";
                            Descripcion = $"Tu solicitud de Prestamo fue Aceptada. Acepta las Condiciones como las fechas de Pago.";
                            await _context.SaveChangesAsync();
                            await new NotificacionesServices(_context).CrearNotificacion(4, IdUser, IdActividad, Razon, Descripcion, pobj.IdUser.ToString(), new ActNotificacione());
                            await new EmailSendServices().EnviarCorreoUsuario(pobj.IdUser, 11, Descripcion);

                        }
                        else if(PEstado != "ACEPTADO" && PEstado =="DENEGADO")
                        {
                            var pobj = (ActPrestamo)new PrestamosRepository().OperacionesPrestamos(2, Id, 0, "");
                            actPrestamo.IdPres = pobj.IdPres;
                            actPrestamo.IdUser = pobj.IdUser;
                            actPrestamo.Valor = pobj.Valor;
                            actPrestamo.FechaGeneracion = pobj.FechaGeneracion;
                            actPrestamo.FechaEntregaDinero = pobj.FechaEntregaDinero;
                            actPrestamo.FechaPagoTotalPrestamo = FechaPagoTotalPrestamo;
                            actPrestamo.FechaInicioPagoCuotas = FechaInicioPagoCuotas;
                            actPrestamo.TipoCuota = "";
                            actPrestamo.Estado = PEstado;
                            _context.Update(actPrestamo);
                            Razon = $"Solicitud de Prestamos DENEGADO";
                            Descripcion = $"Tu solicitud de Prestamo fue DENEGADO.";
                            await _context.SaveChangesAsync();
                            await new NotificacionesServices(_context).CrearNotificacion(4, IdUser, IdActividad, Razon, Descripcion, pobj.IdUser.ToString(), new ActNotificacione());
                            await new EmailSendServices().EnviarCorreoUsuario(pobj.IdUser, 11, Descripcion);
                        }
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine($"Hubo un problema al actualizar el registro del Estado de Prestamo en el Id.{Id}");
                        Console.WriteLine($"Detalles del error: {ex.Message}");
                        return RedirectToAction("Error", "Home");
                    }
                }
            }
            return View(actPrestamo);
        }
    }
}
