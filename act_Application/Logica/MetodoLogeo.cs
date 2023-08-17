using System.Data;
using act_Application.Models;
using MySql.Data.MySqlClient;
using act_Application.Helper;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using act_Application.Services;

namespace act_Application.Logica
{
    public class MetodoLogeo
    {
        public ActUser EncontrarUser(string Correo, string Contrasena)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string userQuery = ConfigReader.GetQuery("SelectUser");

            ActUser objeto = new ActUser();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = userQuery;
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Correo", Correo);
                cmd.Parameters.AddWithValue("@Contrasena", Contrasena);
                cmd.CommandType = CommandType.Text;

                connection.Open();

                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        objeto = new ActUser()
                        {
                            Id = rd.GetInt32(rd.GetOrdinal("Id")),
                            Cedula = rd["Cedula"].ToString(),
                            Correo = rd["Correo"].ToString(),
                            NombreYapellido = rd["NombreYApellido"].ToString(),
                            Celular = rd["Celular"].ToString(),
                            Contrasena = rd["Contrasena"].ToString(),
                            TipoUser = rd["TipoUser"].ToString(),
                            IdSocio = rd.GetInt32(rd.GetOrdinal("IdSocio")),
                        };

                    }
                }
            }
            return objeto;
        }
        public int ObtenerIdRolUsuario(int userId)
        {
            int idRol = 0; // Valor por defecto en caso de que no se encuentre el IdRol

            string connectionString = AppSettingsHelper.GetConnectionString();
            string roleUserQuery = ConfigReader.GetQuery("SelectUserRole");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string rolQuery = roleUserQuery;
                using (MySqlCommand rolCmd = new MySqlCommand(rolQuery, connection))
                {
                    rolCmd.Parameters.AddWithValue("@IdUser", userId);
                    rolCmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = rolCmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            idRol = rd.GetInt32("IdRol");
                        }
                    }
                }
            }

            return idRol;
        }
        public ActRol DatosRolesUser(int idRol)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string roleQuery = ConfigReader.GetQuery("SelectRol"); ;

            ActRol objetoRol = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(roleQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            objetoRol = new ActRol()
                            {
                                Id = rd.GetInt32("Id"),
                                NombreRol = rd["NombreRol"].ToString(),
                                DescripcionRol = rd["DescripcionRol"].ToString()
                            };
                        }
                    }
                }
            }

            return objetoRol;
        }
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
            SmtpConfig smtpConfig = SmtpConfig.LoadConfig("Data/Config/smtpconfig.json");
            string correoDestino = CorreoHelper.ObtenerCorreoDestino(); // Obtener el correo de destino dinámicamente

            using (SmtpClient smtpClient = new SmtpClient(smtpConfig.Server, smtpConfig.Port))
            {
                smtpClient.Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password);
                smtpClient.EnableSsl = true;

                string subject = "act_Application - Notificacion de Inicio de Sesion";
                string body = $"El/La señor@ '{usuario.NombreYapellido}' con el correo '{usuario.Correo}' ha iniciado sesión en la aplicación.";

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
