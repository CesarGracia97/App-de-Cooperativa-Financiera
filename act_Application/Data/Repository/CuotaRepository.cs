using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class CuotaRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        public ActCuota GetDataCuotasUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery1( 1, "CUOT", "DBQC_SelectCoutasUser");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    cmd.Parameters.AddWithValue("@IdUser", IdUser);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActCuota obj = MapToCuota(reader);
                            return obj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDataCuotasUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        public ActCuota GetIdDataCuotaUser(int Id)
        {
            try
            {
                string Query = ConfigReader.GetQuery1( 1, "CUOT", "DBQC_SelectIdCouta");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActCuota obj = MapToCuota(reader);
                            return obj;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetIdDataCuotaUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        public int H_GetLastIdCouta(int IdUser)
        {
            int Id = -1;
            try
            {
                string Query = ConfigReader.GetQuery1( 2, "", "SelectLastIdCoutaUser");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
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
                Console.WriteLine("H_GetLastIdCouta | Error. ");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return Id;
            }

        }
        public List<ActCuota> SA_GetDateCuotasAll()
        {
            List<ActCuota> cuotasList = new List<ActCuota>();
            try
            {
                string Query = ConfigReader.GetQuery1( 3, "", "ATQ_SelectDateCuotasAll");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActCuota obj = MapToCuota(reader);
                            cuotasList.Add(obj);
                        }
                    }
                }
                return cuotasList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SA_GetDateCuotasAll | Error \n");
                Console.WriteLine($"Detalles del error: " + ex.Message);
                return null;
            }
        }
        private ActCuota MapToCuota(MySqlDataReader reader)
        {
            return new ActCuota
            {
                Id = Convert.ToInt32(reader["Id"]),
                IdCuot = Convert.ToString(reader["IdCuot"]),
                IdUser = Convert.ToInt32(reader["IdUser"]),
                IdPrestamo = Convert.ToInt32(reader["IdPrestamo"]),
                FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                FechaCuota = Convert.ToDateTime(reader["FechaCuota"]),
                Valor = Convert.ToDecimal(reader["Valor"]),
                Estado = Convert.ToString(reader["Estado"]),
                FechaPago = Convert.ToString(reader["CBancoOrigen"]),
                CBancoOrigen = Convert.ToString(reader["CBancoOrigen"]),
                NBancoOrigen = Convert.ToString(reader["NBancoOrigen"]),
                CBancoDestino = Convert.ToString(reader["CBancoDestino"]),
                NBancoDestino = Convert.ToString(reader["NBancoDestino"]),
                HistorialValores = Convert.ToString(reader["HistorialValores"]),
                CapturaPantalla = Convert.ToString(reader["CapturaPantalla"])
            };
        }
    }
}
