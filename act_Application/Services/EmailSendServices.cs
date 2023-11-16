using act_Application.Helper;
using act_Application.Models.Sistema.Complementos;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace act_Application.Services
{
    public class EmailSendServices
    {
        /*Envia la solicitud de prestamo*/
        public async Task EnviarCorreoAdmin(int opcion, int IdUser, string Descripcion)
        {
            string correoDestino = string.Empty, subject = string.Empty, body = string.Empty;
            switch (opcion)
            {
                case 1:
                    //Nuevo Usuario
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Solicitud de Ingreso.";
                    body = Descripcion;
                    await EnviarCorreo(correoDestino, subject, body);
                    break;
                 case 2:
                    //Aportaciones
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Aportacion Reciente.";
                    body = Descripcion;
                    break;
                case 3:
                    //Pago de Multas
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Multa Cancelada Recientemente.";
                    body = Descripcion;
                    break;
                case 4:
                    //Pago de Multas
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Multa Abonada  Recientemente.";
                    body = Descripcion;
                    break;
                case 5:
                    //Pago de Cuotas
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Cuota Cancelada  Recientemente.";
                    body = Descripcion;
                    break;
                case 6:
                    //Pago de Cuotas
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Cuota Abonada  Recientemente.";
                    body = Descripcion;
                    break;
                case 7:
                    //Peticion de Prestamos Fase 1
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Solicitud de Prestamo (FASE 1)";
                    body = Descripcion;
                    break;
                case 8:
                    //Peticion de Prestamos Fase 2
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Solicitud de Prestamo (FASE 2)";
                    body = Descripcion;
                    break;
                case 9:
                    //Finalizacion de Evento de Participacion
                    correoDestino = CorreoHelper.ObtenerCorreoDestino();
                    subject = "act - Application: Evento de Participacion (FINALIZADO)";
                    body = Descripcion;
                    break;
                default:
                    Console.WriteLine("EnviarCorreoAdmin - EmailSendServices. Opcion inexistente");
                    break;
            }
            await EnviarCorreo(correoDestino, subject, body);

        }

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
