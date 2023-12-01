using act_Application.Helper;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class CapturaPantallaRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        private int H_GetDataCapturaPantallaLastIdUser(int IdUser)
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
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nH_GetDataCapturaPantallaLastIdUser|| Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nH_GetDataCapturaPantallaLastIdUser | Error.");
                Console.WriteLine($"\nDetalles del error: {ex.Message}\n");
                return Id;
            }
        }
        public object OperacionesCapPan(int Opcion, int Id, int IdUser)
        {
            try
            {
                switch (Opcion)
                {
                    case 1:
                        return H_GetDataCapturaPantallaLastIdUser(IdUser);
                    default:
                        Console.WriteLine("\n-----------------------------------------");
                        Console.WriteLine("\nOperacionesCapPan || Opcion Inexistente.");
                        Console.WriteLine("\n-----------------------------------------\n");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nOperacionesCapPan || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
    }
}
