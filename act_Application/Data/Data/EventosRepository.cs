using act_Application.Helper;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Data
{
    public class EventosRepository
    {
        public int GetTotalEventos()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectExistsEventos");
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
                Console.WriteLine("Hubo un problema al momento de realizar la consulta de existencia de Eventos e Participacion.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return -1; // Valor negativo para indicar un error
            }
        }
    }
}
