using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Data
{
    public class CuotaRepository
    {
        public ActCuota GetDataCuotasId(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string Query = ConfigReader.GetQuery(1, "SelectCoutasUser");

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
                            ActCuota obj = new ActCuota
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                IdCuot = Convert.ToString(rd["IdCuot"]),
                                IdUser = Convert.ToInt32(rd["IdUser"]),
                                IdPrestamo = Convert.ToInt32(rd["IdPrestamo"]),
                                FechaGeneracion = Convert.ToDateTime(rd["FechaGeneracion"]),
                                FechaCuota = Convert.ToDateTime(rd["FechaCuota"]),
                                Valor = Convert.ToDecimal(rd["Valor"]),
                                Estado = Convert.ToString(rd["Estado"]),
                                FechaPago = Convert.ToString(rd["CBancoOrigen"]),
                                CBancoOrigen = Convert.ToString(rd["CBancoOrigen"]),
                                NBancoOrigen = Convert.ToString(rd["NBancoOrigen"]),
                                CBancoDestino = Convert.ToString(rd["CBancoDestino"]),
                                NBancoDestino = Convert.ToString(rd["NBancoDestino"]),
                                HistorialValores = Convert.ToString(rd["HistorialValores"]),
                                CapturaPantalla = Convert.ToString(rd["CapturaPantalla"])
                            };

                            return obj;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de Cuotas");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }

            return null;
        }
    }
}
