using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data
{
    public class AportacionRepository
    {
        public int TotalAportaciones { get; set; }
        public List <ActAportacione> Aportes { get; set; }

        public AportacionRepository GetDataAportaciones()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            AportacionRepository result = new AportacionRepository();
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
                            result.TotalAportaciones = Convert.ToInt32(reader["TotalAportaciones"]);
                            List<ActAportacione> aportes = new List<ActAportacione>();

                            do
                            {
                                ActAportacione apo = new ActAportacione
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    Razon = reader["Razon"].ToString(),
                                    Aprobacion = reader["Aprobacion"].ToString(),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    FechaAportacion = Convert.ToDateTime(reader["FechaAportacion"]),
                                    CapturaPantalla = Convert.IsDBNull(reader.GetOrdinal("CapturaPantalla")) ? null : (byte[])reader["CapturaPantalla"],
                                    NombreUsuario = reader["Razon"].ToString()
                                };
                                aportes.Add(apo);
                            } while (reader.Read());

                            result.Aportes = aportes;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de las aportaciones.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                result.TotalAportaciones = -1; // Valor negativo para indicar un error
                result.Aportes = null;
            }
            return result;
        }
        public List<ActCuentaDestino> ObtenerElementos()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string DestinoQuery = ConfigReader.GetQuery("SelectDestino");

            List<ActCuentaDestino> destinos = new List<ActCuentaDestino>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(DestinoQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        var DestinoElement = rd.Cast<IDataRecord>().Select(r => new
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            Nombre = r["Razon"].ToString(),
                            Valor = Convert.ToSingle(r["Valor"])
                        }).ToList();
                    }
                }
            }

            return destinos;
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
