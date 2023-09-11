using Microsoft.AspNetCore.Mvc;
using act_Application.Data.Data;
using act_Application.Models.BD;
using act_Application.Models.Sistema;
using act_Application.Helper;
using act_Application.Services;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace act_Application.Controllers.General
{
    public class TransaccionesController : Controller
    {
        private readonly DesarrolloContext _context;

        public TransaccionesController(DesarrolloContext context)
        {
            _context = context;
        }

        private List<ListItems> ObtenerItemsRazon()           //Contenido de la Lista Razones
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "PRESTAMO" }
            };
        }

        private List<ListItems> ObtenerItemsCuota()           //Contenido de la Lista Cuotas
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "PAGO UNICO" },
                new ListItems { Id = 2, Nombre = "PAGO MENSUAL" }
            };
        }

        // GET: Aportar/Create
        [Authorize(Policy = "AdminReferenteOnly")]
        public IActionResult Create()
        {

            ViewData["ItemsRazon"] = ObtenerItemsRazon();
            ViewData["ItemsCuota"] = ObtenerItemsCuota();
            return View();
        }

        public string _razonGlobal;
        public int _idTransaccionGlobal;
        public string _descripcionGlobal;

        // POST: Transacciones/Create
        [Authorize(Policy = "AdminReferenteOnly")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Razon,IdUser,Valor,Estado,FechPagoTotalPrestamo,FechaEntregaDinero,FechaIniCoutaPrestamo,TipoCuota")] ActTransaccione actTransaccione)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Establecer las propiedades que deben agregarse automáticamente
                    actTransaccione.IdUser = userId;
                    actTransaccione.FechaPagoTotalPrestamo = DateTime.MinValue;
                    actTransaccione.Estado = "PENDIENTE ADMIN";
                    _razonGlobal = actTransaccione.Razon;

                    _context.Add(actTransaccione);

                    await _context.SaveChangesAsync();

                    _idTransaccionGlobal = actTransaccione.Id;
                    
                    try
                    {
                        await EnviarNotificacionAdministrador(actTransaccione);
                        await EnviarNotificacionUsuario(actTransaccione);
                    }
                    catch(Exception ex)
                    {
                        Thread.Sleep(500);
                        Console.WriteLine("Hubo un problema al enviar la notificación por correo electrónico.");
                        Console.WriteLine("Detalles del error: " + ex.Message);
                    }
                    await CrearNotificacion(new ActNotificacione());
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

        public async Task EnviarNotificacionAdministrador( ActTransaccione actTransaccione)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == "Identificacion")?.Value;
            var userCI = User.Claims.FirstOrDefault(c => c.Type == "CI")?.Value;
            string correoDestino = CorreoHelper.ObtenerCorreoDestino();
            var subject = "act - Application: Solicitud de Prestamo (FASE 1)";
            var body = $"El Usuario {userIdentificacion}, con correo electronico {userEmail} y C.I. {userCI} ha solicitado un préstamo:\n" +
                       $"Detalles:\n" +
                       $"Valor: {actTransaccione.Valor}\n" +
                       $"Fecha de Entrega de Dinero: {actTransaccione.FechaEntregaDinero}\n" +
                       $"Fecha de Inicio de Cuotas: {actTransaccione.FechaIniCoutaPrestamo}\n" +
                       $"Tipo de Cuota: {actTransaccione.TipoCuota}\n" +
                       $"Estado: {actTransaccione.Estado}\n" +
                       $"Fecha de Pago Total del Préstamo: En Espera de Evaluacion" +
                       $"Numero de Cuotas: En Espera de Evaluacion \n"+
                       $"Valor de las Cuotas: En Espera de Evaluacion";
            _descripcionGlobal = body;

            await EnviarCorreo(correoDestino, subject, body);
        }

        private async Task EnviarNotificacionUsuario(ActTransaccione actTransaccione)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(userEmail))
            {
                var subject = "Solicitud de Prestamo (FASE 1)";
                var body = $"Usted a solicitado préstamo con los siguientes detalles:\n\n" +
                           $"Valor: {actTransaccione.Valor}\n" +
                           $"Tipo de Cuota: {actTransaccione.TipoCuota}\n" +
                           $"Fecha de Entrega de Dinero: {actTransaccione.FechaEntregaDinero}\n" +
                           $"Fecha de Inicio de Cuotas: {actTransaccione.FechaIniCoutaPrestamo}\n\n" +
                           $"Detalles en Espera:\n\n" +
                           $"Fecha de Pago Total del Préstamo: En Espera de Evaluacion" +
                           $"Numero de Cuotas: En Espera de Evaluacion \n" +
                           $"Valor de las Cuotas: En Espera de Evaluacion\n\n" +
                           $"Tu solicitud ha sido enviada para evaluación. Espera una respuesta pronto (Tiempo Maximo 3 Dias Habiles).\n\n"+
                           $"-----------------------------------\n"+
                           $"NO RESPONDER POR ESTE MEDIO\n" +
                           $"-----------------------------------";
                await EnviarCorreo(userEmail, subject, body);
            }
        }

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

        private async Task CrearNotificacion([Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino,IdTransacciones,IdAportaciones,IdCuotas ")] ActNotificacione actNotificacione)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                actNotificacione.IdUser = userId;
                actNotificacione.Razon = _razonGlobal;
                actNotificacione.Descripcion = _descripcionGlobal;
                actNotificacione.FechaNotificacion = DateTime.Now;
                actNotificacione.Destino = "ADMINISTRADOR";
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

    }
}
