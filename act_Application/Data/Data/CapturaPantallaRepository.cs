using act_Application.Helper;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Data
{
    public class CapturaPantallaRepository
    {
        public int GetDataCapturaPantallaLastIdUser(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            int Id = -1;
            try
            {
                string Query = ConfigReader.GetQuery(2, "SelectLastCoutaIdUser");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = Query;
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdUse", IdUser);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            Id = Convert.ToInt32(rd["Id"]);
                        };
                    }
                }
                return Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta del Ultimo Id de la Cuota");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return Id;

        }
    }
}
