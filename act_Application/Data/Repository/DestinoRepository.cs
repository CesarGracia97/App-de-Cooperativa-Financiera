using act_Application.Helper;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Repository
{
    public class DestinoRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        public List<ActCuentaDestino> GetDataDestinos()
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
            catch (Exception ex)
            {
                Console.WriteLine("GetDataDestinos | Error");
                Console.WriteLine("Detalles del error: " + ex.Message);
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
    }
}
