using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;

namespace act_Application.Data
{
    public class AportacionRepository
    {
        public int TotalAportaciones { get; set; }
        public List <ActAportacione> Aportacion { get; set; }
        public AportacionRepository GetDataAportaciones()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            AportacionRepository result = new AportacionRepository();
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de eventos.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                result.TotalAportaciones = -1; // Valor negativo para indicar un error
            }

        }
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
                Console.WriteLine("Hubo un problema al momento de realizar la consulta de las aportaciones.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return -1; // Valor negativo para indicar un error
            }
        }
    }
}
