using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class CuotaRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        private bool GetExist_CuotasUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "CUOT", "DBQC_SelectCoutasUser");
                int totalCuotas = 0;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    cmd.Parameters.AddWithValue("@IdUser", IdUser);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalCuotas = Convert.ToInt32(reader["TotalCuotas"]);
                        }
                    }
                }
                return totalCuotas > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExist_CuotasUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetExist_CuotasUser | Error \n");
                Console.WriteLine($"Detalles del error: " + ex.Message);
                return false;
            }
        }
        private List<ActCuota> GetData_CuotasUser(int IdUser)
        {
            try
            {
                List<ActCuota> listCuota = new List<ActCuota>();
                string Query = ConfigReader.GetQuery( 1, "CUOT", "DBQC_SelectCoutasUser");

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
                            listCuota.Add(obj);
                        }
                    }
                }
                return listCuota;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_CuotasUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetData_CuotasUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        private ActCuota GetData_IdCuotaUser(int Id)
        {
            try
            {
                string Query = ConfigReader.GetQuery( 1, "CUOT", "DBQC_SelectIdCouta");

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
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_IdCuotaUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetData_IdCuotaUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        private int Auto_GetData_LastIdCouta(int IdUser)
        {
            int Id = -1;
            try
            {
                string Query = ConfigReader.GetQuery( 2, "", "SelectLastIdCoutaUser");

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
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nH_GetData_LastIdCouta || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Auto_GetData_LastIdCouta | Error. ");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return Id;
            }

        }
        private int GetData_IdCuota(int IdCuot) // Obtienes el Id de un registro de cuota por medio de su IdPersonalizado.
        {

        }
        private List<ActCuota> SA_GetData_DateCuotasAll()
        {
            List<ActCuota> cuotasList = new List<ActCuota>();
            try
            {
                string Query = ConfigReader.GetQuery( 3, "", "ATQ_SelectDateCuotasAll");

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
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nSA_GetData_DateCuotasAll || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SA_GetData_DateCuotasAll | Error \n");
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
        public object OperacionesCuotas (int Opciones, int Id, int IdUser)
        {
            try
            {
                switch (Opciones)
                {
                    case 1:
                        return GetData_CuotasUser(IdUser);
                    case 2:
                        return GetData_IdCuotaUser(Id);
                    case 3:
                        return Auto_GetData_LastIdCouta(IdUser);
                    case 4:
                        return SA_GetData_DateCuotasAll();
                    case 5:
                        return GetExist_CuotasUser(IdUser);
                    case 6:
                        return GetData_IdCuota();
                    default:
                        Console.WriteLine("\n-----------------------------------------");
                        Console.WriteLine("\nOperacionesCuotas || Opcion Inexistente.");
                        Console.WriteLine("\n-----------------------------------------\n");
                        return null;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nOperacionesCuotas || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
    }
}
