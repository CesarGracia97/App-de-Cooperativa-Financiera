using System;

using act_Application.Helper;
using MySql.Data.MySqlClient;

namespace act_Application.Data
{
    public class AportacionRepository
    {

        public int GetTotalAportaciones()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectExistenciaAportaciones");
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
                // Maneja las excepciones aquí si es necesario
                return -1; // Valor negativo para indicar un error
            }
        }
    }
}
