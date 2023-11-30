using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using act_Application.Models.BD;
using act_Application.Helper;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Logic
{
    public class MetodoLogeo
    {
        public bool ValidarCorreo(string correo)
        {
            // Validación del formato de correo electrónico utilizando la expresión regular
            if (!Regex.IsMatch(correo, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"))
            {
                return false;
            }

            // Otros controles de caracteres especiales si es necesario

            return true;
        }
        public void EnviarNotificacionInicioSesion(ActUser usuario)
        {
            SmtpConfig smtpConfig = SmtpConfig.LoadConfig("Data/json/smtp.json");
            string correoDestino = CorreoHelper.ObtenerCorreoDestino(); // Obtener el correo de destino dinámicamente

            using (SmtpClient smtpClient = new SmtpClient(smtpConfig.Server, smtpConfig.Port))
            {
                smtpClient.Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password);
                smtpClient.EnableSsl = true;

                string subject = "act_Application - Notificacion de Inicio de Sesion";
                string body = $"El '{usuario.NombreYapellido}' con el correo '{usuario.Correo}' ha iniciado sesión en la aplicación.";

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpConfig.Username),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(correoDestino); // Usar el correo de destino obtenido

                smtpClient.Send(mailMessage);
            }
        }
    }
}
