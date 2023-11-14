using System.Data;
using MySql.Data.MySqlClient;

namespace act_Application.Helper
{
    public class CorreoHelper
    {
        public static string ObtenerCorreoDestino()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string emailQuery = ConfigReader.GetQuery(1, "SelectEmail");

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