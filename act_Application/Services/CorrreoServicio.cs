using System.Data;
using MySql.Data.MySqlClient;
using act_Application.Helper;
using System.Configuration;

namespace act_Application.Services
{
    public class CorreoHelper
    {
        public static string ObtenerCorreoDestino()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string emailQuery = ConfigReader.GetQuery("SelectEmail");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(emailQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    string correoDestino = cmd.ExecuteScalar()?.ToString();
                    return correoDestino;
                }
            }
        }
    }
}