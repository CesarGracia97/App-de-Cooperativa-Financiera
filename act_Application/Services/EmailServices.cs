using System.Net;
using System.Net.Mail;
using act_Application.Helper;

namespace act_Application.Services
{
    public class EmailService
    {
        public readonly SmtpSettings _smtpConfig;

        public EmailService()
        {
            _smtpConfig = SmtpSettings.Load();
        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(_smtpConfig.Server)
                {
                    Port = _smtpConfig.Port,
                    Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password),
                    EnableSsl = true
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpConfig.Username),
                    Subject = subject,
                    Body = body
                };
                mailMessage.To.Add(to);

                smtpClient.Send(mailMessage);
                Console.WriteLine("Correo enviado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
    }
}