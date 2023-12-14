using act_Application.Data.Repository;
using act_Application.Helper;
using act_Application.Models.Sistema.Complementos;
using System.Net.Mail;
using System.Net;

namespace act_Application.Services.ServiciosAplicativos
{
    public class EmailSendServices
    {
        /*Envia la solicitud de prestamo*/
        public async Task EnviarCorreoAdmin(int opcion, string Descripcion)
        {
            string correoDestino = CorreoHelper.ObtenerCorreoDestino(), subject = string.Empty, body = string.Empty;
            switch (opcion)
            {
                case 1:
                    //Nuevo Usuario
                    subject = "act - Application: Solicitud de Ingreso.";
                    body = Descripcion;
                    break;
                case 2:
                    //Aportaciones
                    subject = "act - Application: Aportacion Reciente.";
                    body = Descripcion;
                    break;
                case 3:
                    //Pago de Multas
                    subject = "act - Application: Multa Cancelada Recientemente.";
                    body = Descripcion;
                    break;
                case 4:
                    //Pago de Multas
                    subject = "act - Application: Multa Abonada  Recientemente.";
                    body = Descripcion;
                    break;
                case 5:
                    //Pago de Cuotas
                    subject = "act - Application: Cuota Cancelada  Recientemente.";
                    body = Descripcion;
                    break;
                case 6:
                    //Pago de Cuotas
                    subject = "act - Application: Cuota Abonada  Recientemente.";
                    body = Descripcion;
                    break;
                case 7:
                    //Peticion de Prestamos Fase 1
                    subject = "act - Application: Solicitud de Prestamo (FASE 1)";
                    body = Descripcion;
                    break;
                case 8:
                    //Peticion de Prestamos Fase 2
                    subject = "act - Application: Solicitud de Prestamo (FASE 2)";
                    body = Descripcion;
                    break;
                case 9:
                    //Finalizacion de Evento de Participacion
                    subject = "act - Application: Evento de Participacion (FINALIZADO)";
                    body = Descripcion;
                    break;
                default:
                    Console.WriteLine("EnviarCorreoAdmin - EmailSendServices. Opcion inexistente");
                    break;
            }
            await EnviarCorreo(correoDestino, subject, body);

        }
        public async Task EnviarCorreoUsuario(int IdUser, int opcion, string Descripcion)
        {
            string userEmail = (string) new UsuarioRepository().OperacionesUsuario(4, 0, IdUser, "", ""), subject = string.Empty;
            if (userEmail != null)
            {
                switch (opcion)
                {
                    case 1:
                        //Solicitud de Prestamos Fase 1
                        subject = $"act - Application: Solicitud de Prestamo (FASE 1) ({DateTime.Now})";
                        break;
                    case 2:
                        //Solicitud de Prestamos Fase 2
                        subject = $"act - Application: Solicitud de Prestamo (FASE 2) ({DateTime.Now})";
                        break;
                    case 3:
                        //Movimiento de Aportacion
                        subject = $"act - Application: Aportacion Reciente ({DateTime.Now})";
                        break;
                    case 4:
                        //Abono de Multa.
                        subject = $"act - Application: Abono de Multa Reciente ({DateTime.Now})";
                        break;
                    case 5:
                        //Cancelacion de Multa
                        subject = $"act - Application: Cancelacion de Multa ({DateTime.Now})";
                        break;
                    case 6:
                        //Abono de Cuota
                        subject = $"act - Application: Abono de Cuota Reciente ({DateTime.Now})";
                        break;
                    case 7:
                        //Cancelacion de Cuota
                        subject = $"act - Application: Cancelacion de Cuota  ({DateTime.Now})";
                        break;
                    case 8:
                        //Garante de Participacion
                        subject = $"act - Application: Garante de Participacion ({DateTime.Now})";
                        break;
                    case 9:
                        //Multa Aplicada
                        subject = $"act - Application: Aplicacion de Multa ({DateTime.Now})";
                        break;
                    case 10:
                        //Cuenta Aceptada
                        subject = $"act - Application: Cuenta Aceptada ({DateTime.Now})";
                        break;
                    case 11:
                        //Cuenta Rechazada.
                        subject = $"act - Application: Cuenta Rechazada ({DateTime.Now})";
                        break;
                    case 12:
                        //PRestamo Rechazado.
                        subject = $"act - Application: Solicitud de Prestamo RECHAZADA ({DateTime.Now})";
                        break;
                    default:
                        Console.WriteLine("EnviarCorreoUsuario - EmailSendServices. Opcion inexistente");
                        break;
                }
                await EnviarCorreo(userEmail, subject, Descripcion);
            }
            else
            {
                Console.WriteLine("EnviarCorreoUsuario - EmailSendServices. Error al obtener el Correo");
            }
        }
        /*Envia el Correo*/
        private async Task EnviarCorreo(string destinatario, string asunto, string mensaje)
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
