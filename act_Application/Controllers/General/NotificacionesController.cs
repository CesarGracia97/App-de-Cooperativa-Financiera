using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using act_Application.Models.Sistema.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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
                if(User.HasClaim("Rol", "Administrador"))
                {
                    ViewData["ItemEstado"] = ItemsTipoEstado();
                    ViewData["ItemTipoUs"] = ItemsTipoUsuario();
                    var notiAdmi = (List<ActNotificacione>) new NotificacionesRepository().OperacionesNotificaciones(3, 0, 0);
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
                        var notiUser = (List<ActNotificacione>) new NotificacionesRepository().OperacionesNotificaciones( 4, 0, Bandera);
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
        public async Task<IActionResult> Visualizado(int IdN, int IdA, int Opcion, [Bind("Id,IdActividad,FechaGeneracion,Razon,Descripcion,Destino,Visto")] ActNotificacione actNotificacione, [Bind("Id,Cedula,NombreYApellido,Contrasena,Celular,TipoUser,IdSocio,FotoPerfil")] ActUser actUser, [Bind("Id,IdPres,IdUser,Valor,FechaGeneracion,FechaEntregaDinero,FechaInicioPagoCuotas,FechaPagoTotalPrestamo,TipoCuota,Estado")] ActPrestamo actPrestamo)
        {
            if (IdN != actNotificacione.Id)
            {
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var nobj = (ActNotificacione)new NotificacionesRepository().OperacionesNotificaciones(5, IdN, 0);
                    switch (Opcion)
                    {
                        case 1/*Solo Actualiza la Notificacion a Vista.*/:
                            //---Actualizacion Visualizacion
                            actNotificacione.IdUser = nobj.IdUser;
                            actNotificacione.IdActividad = nobj.IdActividad;
                            actNotificacione.FechaGeneracion = nobj.FechaGeneracion;
                            actNotificacione.Razon = nobj.Razon;
                            actNotificacione.Descripcion = nobj.Descripcion;
                            actNotificacione.Destino = nobj.Destino;
                            actNotificacione.Visto = "SI";
                            _context.Update(actNotificacione);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index", "Notificaciones");
                        case 2/*Confirmar o Rechazar Nuevo Usuario*/:
                            //---Actualizacion Visualizacion
                            actNotificacione.IdUser = nobj.IdUser;
                            actNotificacione.IdActividad = nobj.IdActividad;
                            actNotificacione.FechaGeneracion = nobj.FechaGeneracion;
                            actNotificacione.Razon = nobj.Razon;
                            actNotificacione.Descripcion = nobj.Descripcion;
                            actNotificacione.Destino = nobj.Destino;
                            actNotificacione.Visto = "SI";
                            _context.Update(actNotificacione);
                            await _context.SaveChangesAsync();
                            //----Actualizar Estado de Usuario
                            if (IdA != actUser.Id)
                            {
                                return RedirectToAction("Error", "Home");
                            }
                            if (ModelState.IsValid)
                            {
                                try
                                {
                                    
                                }
                                catch (DbUpdateConcurrencyException ex)
                                {
                                    Console.WriteLine($"Hubo un problema al actualizar el registro del Estado del Usuario en el Id.{IdA}");
                                    Console.WriteLine($"Detalles del error: {ex.Message}");
                                    return RedirectToAction("Error", "Home");
                                }
                            }
                            return RedirectToAction("Index", "Notificaciones");
                        case 3/*Solicitud de Prestamo*/:
                            //---Actualizacion Visualizacion
                            actNotificacione.IdUser = nobj.IdUser;
                            actNotificacione.IdActividad = nobj.IdActividad;
                            actNotificacione.FechaGeneracion = nobj.FechaGeneracion;
                            actNotificacione.Razon = nobj.Razon;
                            actNotificacione.Descripcion = nobj.Descripcion;
                            actNotificacione.Destino = nobj.Destino;
                            actNotificacione.Visto = "SI";
                            _context.Update(actNotificacione);
                            await _context.SaveChangesAsync();
                            if (IdA != actPrestamo.Id)
                            {
                                return RedirectToAction("Error", "Home");
                            }
                            if (ModelState.IsValid)
                            {
                                try
                                {

                                }
                                catch (DbUpdateConcurrencyException ex)
                                {
                                    Console.WriteLine($"Hubo un problema al actualizar el registro del Estado de Prestamo en el Id.{IdA}");
                                    Console.WriteLine($"Detalles del error: {ex.Message}");
                                    return RedirectToAction("Error", "Home");
                                }
                            }
                            return RedirectToAction("Index", "Notificaciones");
                        default:
                            break;
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
    }
}
