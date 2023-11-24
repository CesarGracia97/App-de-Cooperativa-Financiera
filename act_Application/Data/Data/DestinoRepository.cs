using act_Application.Helper;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Data
{
    public class DestinoRepository
    {
        public class Repositorio
        {
            public List<ActCuentaDestino> GetDataDestinos()
            {
                string connectionString = AppSettingsHelper.GetConnectionString();
                try
                {
                    string Query = ConfigReader.GetQuery(1, "SelectDestino");
                    List<ActCuentaDestino> cuentasDestino = new List<ActCuentaDestino>();

                    using (MySqlConnection conexion = new MySqlConnection(connectionString))
                    {
                        conexion.Open();

                        MySqlCommand cmd = new MySqlCommand(Query, conexion);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ActCuentaDestino cuentaDestino = new ActCuentaDestino
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    NumeroCuentaB = Convert.ToString(reader["NumeroCuentaB"]),
                                    NombreBanco = Convert.ToString(reader["NombreBanco"])
                                };

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
                }
                return null;
            }
        }
    }
}
