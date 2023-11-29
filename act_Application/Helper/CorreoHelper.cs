using System.Data;
using MySql.Data.MySqlClient;

namespace act_Application.Helper
{
    public class CorreoHelper
    {
        private readonly static string connectionString = AppSettingsHelper.GetConnectionString();
        private readonly static string Query = ConfigReader.GetQuery1(1, "USER", "DBQU_SelectEmail");
        public static string ObtenerCorreoDestino()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(Query, connection))
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