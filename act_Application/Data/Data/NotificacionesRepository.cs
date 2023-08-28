using act_Application.Helper;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Data
{
    public class NotificacionesRepository
    {
        public int GetTotalNotificacionesAdministrador()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectExistenciaNotifiAdmin"); // Asegúrate de tener la consulta SQL correcta para obtener la existencia de multas
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo En la consulta de la Notificacion");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return -1; // Valor negativo para indicar un error
            }
        }

    }
}
