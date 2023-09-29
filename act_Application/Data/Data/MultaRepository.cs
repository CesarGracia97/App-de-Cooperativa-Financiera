using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Data
{
    public class MultaRepository
    {
        public int TotalMultas { get; set; }
        public List<ActMulta> Multas { get; set; }

        public MultaRepository GetDataMulta()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            MultaRepository result = new MultaRepository();
            try
            {
                string query = ConfigReader.GetQuery("");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.TotalMultas = Convert.ToInt32(reader["TotalMultas"]);
                            List<ActMulta> multas = new List<ActMulta>();

                            do
                            {
                                ActMulta mul = new ActMulta
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    NombreUsuario = reader["Razon"].ToString(),
                                    FechaMulta = Convert.ToDateTime(reader["FechaMulta"])
                                };
                                multas.Add(mul);
                            } while (reader.Read());

                            result.Multas = multas;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de las aportaciones.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                result.TotalMultas = -1; // Valor negativo para indicar un error
                result.Multas = null;
            }
            return result;

        }

        public int GetTotalMultas()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectExistenciaMultas"); // Asegúrate de tener la consulta SQL correcta para obtener la existencia de multas
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
                Console.WriteLine("Hubo un problema al momento de  realizar la consulta de la Multa");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return -1; // Valor negativo para indicar un error
            }
        }
    }
}
