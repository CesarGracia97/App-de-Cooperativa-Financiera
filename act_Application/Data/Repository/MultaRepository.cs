using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class MultaRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        private bool GetExist_Multas() //Existen multas a nivel global 
        {
            try
            {
                string Query = ConfigReader.GetQuery( 1, "MULT", "DBQM_SelectMulta");

                int totalMultas = 0; // Variable para almacenar el valor de TotalAportaciones

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(Query, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        connection.Open();

                        using (MySqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read()) // Avanzar al primer registro
                            {
                                totalMultas = Convert.ToInt32(rd["TotalMultas"]);
                            }
                        }
                    }
                }
                // Si totalAportaciones es mayor que 0, devuelve true, de lo contrario, devuelve false.
                return totalMultas > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExist_Multas || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetExist_Multas || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return false;
            }
        }
        private bool GetExist_MultasUser(int IdUser) //Existen multas del Usuario 
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "MULT", "DBQM_SelectMultasUser");
                int totalMultas = 0;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                totalMultas = Convert.ToInt32(reader["TotalMultas"]);
                        }
                    }
                }
                return totalMultas > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExist_MultasUser|| Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetExist_MultasUser || Error");
                Console.WriteLine("Razon del Error: " + ex.Message);
                return false;
            }
        }
        private List<ActMulta> GetData_Multas() //Obtener todas los registro de Multas. 
        {
            try
            {

                List<ActMulta> multas = new List<ActMulta>();
                string Query = ConfigReader.GetQuery( 1, "MULT", "DBQM_SelectMulta");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(Query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();
                        using (MySqlDataReader rd = cmd.ExecuteReader())
                        {
                            var multasPorUsuario = rd.Cast<IDataRecord>()
                                .Select(r => new
                                {
                                    Id = Convert.ToInt32(r["Id"]),
                                    IdMult = Convert.ToString(r["IdMult"]),
                                    IdUser = Convert.ToInt32(r["IdUser"]),
                                    FechaGeneracion = Convert.ToDateTime(r["FechaGeneracion"]),
                                    Cuadrante = Convert.ToString(r["Cuadrante"]),
                                    Razon = Convert.ToString(r["Razon"]),
                                    Valor = Convert.ToDecimal(r["Valor"]),
                                    Estado = Convert.ToString(r["Estado"]),
                                    FechaPago = Convert.ToString(r["FechaPago"]),
                                    CBancoOrigen = Convert.ToString(r["CBancoOrigen"]),
                                    NBancoOrigen = Convert.ToString(r["NBancoOrigen"]),
                                    CBancoDestino = Convert.ToString(r["CBancoDestino"]),
                                    NBancoDestino = Convert.ToString(r["NBancoDestino"]),
                                    HisotiralValores = Convert.ToString(r["HisotiralValores"]),
                                    CapturaPantalla = Convert.ToString(r["CapturaPantalla"]),
                                    NombreUsuario = Convert.ToString(r["NombreUsuario"])
                                })
                                .ToList();

                            var multasAgrupadas = multasPorUsuario
                                .GroupBy(m => new { m.NombreUsuario, m.FechaGeneracion.Month }) // Agrupamos por NombreUsuario y Mes
                                .ToList();

                            foreach (var group in multasAgrupadas)
                            {
                                var multa = new ActMulta
                                {
                                    Id = group.First().Id,
                                    IdUser = group.First().IdUser,
                                    FechaGeneracion = group.First().FechaGeneracion,
                                    NombreUsuario = group.Key.NombreUsuario
                                };

                                multa.NumeroMultas = group.Count();
                                multa.DetallesMulta = group.Select(a => new DetalleMulta
                                {
                                    Valor = (decimal)a.Valor,
                                    FechaMulta = a.FechaGeneracion,
                                    Cuadrante = a.FechaGeneracion.Day <= 15 ? 1 : 2
                                }).ToList();

                                // Calculamos el valor total de las multas en el mes
                                multa.Valor = group.Sum(m => m.Valor);

                                multas.Add(multa);
                            }
                        }
                    }
                }
                return multas;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_Multas|| Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetData_Multas || Error");
                Console.WriteLine("Razon del Error: " + ex.Message);
                return null;
            }
        }
        private List<ActMulta> GetData_MultasUser(int IdUser) //Obtener todos los registros de Multas de un Usuario en Especifico 
        {
            try
            {
                List<ActMulta> List = new List<ActMulta>();
                string Query = ConfigReader.GetQuery(1, "MULT", "DBQM_SelectMultasUser");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ActMulta obj = MapToMulta(reader);
                                List.Add(obj);
                            }
                        }
                    }
                }
                return List;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_MultasUser|| Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nGetDataMultaUser || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
        private ActMulta GetData_MultasId(int Id) //Obtener un registro en Especifico de Multas 
        {
            try
            {
                string Query = ConfigReader.GetQuery( 1, "MULT", "DBQM_SelectMultasId");
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
                            ActMulta obj = MapToMulta(reader);
                            return obj;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_MultasId|| Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetData_MultasId | Error");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        private int A_GetData_LastIdMultaUser(int IdUser) //Obtiene el Id del ultimo registro de la Multa de un Usuario. 
        {
            int Id = -1;
            try
            {
                string Query = ConfigReader.GetQuery( 2, "", "ASQ_SelectLastIdMultaUser");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    cmd.Parameters.AddWithValue("@IdUser", IdUser);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            Id = Convert.ToInt32(rd["Id"]);
                        }
                        return Id;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nA_GetData_LastIdMultaUser|| Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("A_GetData_LastIdMultaUser|| Error");
                Console.WriteLine("Razon del Error: " + ex.Message);
            }
            return Id;
        }
        private ActMulta MapToMulta(MySqlDataReader reader)
        {
            return new ActMulta
            {
                Id = Convert.ToInt32(reader["Id"]),
                IdMult = Convert.ToString(reader["IdMult"]),
                IdUser = Convert.ToInt32(reader["IdUser"]),
                FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                Cuadrante = Convert.ToString(reader["Cuadrante"]),
                Razon = Convert.ToString(reader["Razon"]),
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
        public object OperacionesMultas(int Opcion, int Id, int IdUser)
        {
            try
            {
                switch (Opcion)
                {
                    case 1:
                        return GetExist_Multas();
                    case 2:
                        return GetExist_MultasUser(IdUser);
                    case 3:
                        return GetData_Multas();
                    case 4:
                        return GetData_MultasUser(IdUser);
                    case 5:
                        return GetData_MultasId(Id);
                    case 6:
                        return A_GetData_LastIdMultaUser(IdUser);
                    default:
                        Console.WriteLine("\n-----------------------------------------");
                        Console.WriteLine("\nOperacionesMulta || Opcion Inexistente.");
                        Console.WriteLine("\n-----------------------------------------\n");
                        return null;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nOperacionesMulta || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
    }
}
