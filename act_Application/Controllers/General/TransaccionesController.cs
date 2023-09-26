using Microsoft.AspNetCore.Mvc;
using act_Application.Data.Data;
using act_Application.Models.BD;
using act_Application.Models.Sistema;
using act_Application.Services;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Controllers.General
{
    public class TransaccionesController : Controller
    {
        private readonly DesarrolloContext _context;

        public TransaccionesController(DesarrolloContext context)
        {
            _context = context;
        }


        //Contenido de la Lista Razones
        private List<ListItems> ObtenerItemsRazon()
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "PRESTAMO" }
            };
        }


        //Contenido de la Lista Cuotas
        private List<ListItems> ObtenerItemsCuota()
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "PAGO UNICO" },
                new ListItems { Id = 2, Nombre = "PAGO MENSUAL" }
            };
        }


        //Datos directos kue se obtiene por las listas
        [Authorize(Policy = "AdminReferenteOnly")]
        public IActionResult Transacciones()
        {
            ViewData["ItemsRazon"] = ObtenerItemsRazon();
            ViewData["ItemsCuota"] = ObtenerItemsCuota();
            return View();
        }


        /*Crea una solicitud de prestamo*/
        [Authorize(Policy = "AdminReferenteOnly")][HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Razon,IdUser,Valor,Estado,FechPagoTotalPrestamo,FechaEntregaDinero,FechaIniCoutaPrestamo,TipoCuota,IdParticipantes,FechaGeneracion")] ActTransaccione actTransaccione)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == "Identificacion")?.Value;
                    var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var userCI = User.Claims.FirstOrDefault(c => c.Type == "CI")?.Value;

                    // Establecer las propiedades que deben agregarse automáticamente
                    actTransaccione.IdUser = userId;
                    actTransaccione.FechaPagoTotalPrestamo = DateTime.MinValue;
                    actTransaccione.Estado = "PENDIENTE ADMIN";
                    actTransaccione.FechaGeneracion = DateTime.Now;
                    actTransaccione.IdParticipantes = 0;
                    _context.Add(actTransaccione);

                    await _context.SaveChangesAsync();

                    try
                    {
                        string Descripcion = $"El usuario {userIdentificacion} con Correo {userEmail} y C.I. {userCI} esta solicitando un prestamo de $ {actTransaccione.Valor} USD," +
                                                $"con fecha de entrega para el dia {actTransaccione.FechaEntregaDinero}, e inicio de pago de la deuda para el dia {actTransaccione.FechaIniCoutaPrestamo}\n" +
                                                $"Estado: {actTransaccione.Estado}\n" + $"Tipo de Cuota: {actTransaccione.TipoCuota}";

                        await CrearNotificacion(actTransaccione.Razon, Descripcion, actTransaccione.Id, new ActNotificacione());
                        await EnviarCorreoAdmin(Descripcion);
                        await EnviarCorreoUsuario(Descripcion);
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(500);
                        Console.WriteLine("Hubo un problema al enviar la notificación por correo electrónico.");
                        Console.WriteLine("Detalles del error: " + ex.Message);
                    }
                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                    // Manejar el caso en que no se pueda obtener el Id del usuario
                    ModelState.AddModelError("", "Error al obtener el Id del usuario.");
                    Console.WriteLine("Fallo el guardado");
                }
            }
            return View(actTransaccione);
        }


        /*Crea un nuevo registro de Notificacion*/
        private async Task CrearNotificacion(string razon, string descripcion, int idTransaccion, [Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino,IdTransacciones,IdAportaciones,IdCuotas")] ActNotificacione actNotificacione)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                actNotificacione.IdUser = userId;
                actNotificacione.Razon = razon;
                actNotificacione.Descripcion = descripcion;
                actNotificacione.FechaNotificacion = DateTime.Now;
                actNotificacione.Destino = "ADMINISTRADOR";
                actNotificacione.IdTransacciones = idTransaccion;
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


        /*Envia la solicitud de prestamo*/
        public async Task EnviarCorreoAdmin(string Descripcion)
        {
            string correoDestino = CorreoHelper.ObtenerCorreoDestino();
            var subject = "act - Application: Solicitud de Prestamo (FASE 1)";
            var body = Descripcion;

            await EnviarCorreo(correoDestino, subject, body);
        }


        /*Envia una notificacion sobre la solicitud de prestamo*/
        private async Task EnviarCorreoUsuario(string Descripcion)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(userEmail))
            {
                var subject = "Solicitud de Prestamo (FASE 1)";
                var body = Descripcion;
                await EnviarCorreo(userEmail, subject, body);
            }
        }


        /*Envia el Correo*/
        public async Task EnviarCorreo(string destinatario, string asunto, string mensaje)
        {
            var smtpConfig = SmtpConfig.LoadConfig("Data/Config/smtpconfig.json");

            using (var smtpClient = new SmtpClient(smtpConfig.Server, smtpConfig.Port))
            {
                smtpClient.Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpConfig.Username),
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(destinatario);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }


    }
}
