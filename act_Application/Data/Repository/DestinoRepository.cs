using act_Application.Helper;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Repository
{
    public class DestinoRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        private List<ActCuentaDestino> GetDataDestinos()
        {
            try
            {
                string Query = ConfigReader.GetQuery( 1,"DEST", "DBQD_SelectDestino");
                List<ActCuentaDestino> cuentasDestino = new List<ActCuentaDestino>();

                using (MySqlConnection conexion = new MySqlConnection(connectionString))
                {
                    conexion.Open();

                    MySqlCommand cmd = new MySqlCommand(Query, conexion);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActCuentaDestino cuentaDestino = MapToCuentaDestino(reader);
                            cuentasDestino.Add(cuentaDestino);
                        }
                    }
                }
                return cuentasDestino;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetDataDestinos || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetDataDestinos | Error");
                Console.WriteLine($"\nDetalles del error: {ex.Message}\n");
                return null;
            }
        }
        private ActCuentaDestino MapToCuentaDestino(MySqlDataReader reader)
        {
            return new ActCuentaDestino
            {
                Id = Convert.ToInt32(reader["Id"]),
                NumeroCuentaB = Convert.ToString(reader["NumeroCuentaB"]),
                NombreBanco = Convert.ToString(reader["NombreBanco"])
            };
        }
        public object OperacionDestino(int Opcion, int Id, int IdUser)
        {
            try
            {
                switch (Opcion)
                {
                    case 1:
                        return GetDataDestinos();
                    default:
                        Console.WriteLine("\n-----------------------------------------");
                        Console.WriteLine("\nOperacionesDestino || Opcion Inexistente.");
                        Console.WriteLine("\n-----------------------------------------\n");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nOperacionesDestino || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
    }
}
