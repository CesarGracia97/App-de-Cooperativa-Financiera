using act_Application.Helper;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class CapturaPantallaRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        public int H_GetDataCapturaPantallaLastIdUser(int IdUser)
        {
            int Id = -1;
            try
            {
                string Query = ConfigReader.GetQuery( 2, "", "ASQ_SelectLastIdCapturaPantallaUser");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    cmd.Parameters.AddWithValue("@IdUser", IdUser);
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
                Console.WriteLine("H_GetDataCapturaPantallaLastIdUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return Id;
            }
        }
    }
}
