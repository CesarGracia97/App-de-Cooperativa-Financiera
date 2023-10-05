using act_Application.Helper;
using act_Application.Models.BD;
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
                string Query = ConfigReader.GetQuery("SelectDestino");
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
                                Id = reader.GetInt32("Id"),
                                NumeroCuenta = reader.GetString("NumeroCuenta"),
                                NombreBanco = reader.GetString("NombreBanco")
                            };

                            cuentasDestino.Add(cuentaDestino);
                        }
                    }
                }

                return cuentasDestino;
            }
        }
    }
}
