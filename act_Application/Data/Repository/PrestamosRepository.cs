using act_Application.Helper;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace act_Application.Data.Repository
{
    public class PrestamosRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        private int GetData_IdPrestamo_IdPres(string IdPres)
        {
            int Id = 0;
            try
            {
                string Query = ConfigReader.GetQuery(1, "PRES", "DBQP_SelectPrestamoIdPres");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPres", IdPres);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = Convert.ToInt32(reader["Id"]);
                            }
                        }
                    }
                }
                return Id;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_PrestamoIdPres || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetData_PrestamoIdPres || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return Id = -1;
            }
        }
        private ActPrestamo GetData_PrestamoId(int IdPrestamo) //Consulta para obtener todos los datos de una transaccion especifica
        {
            try
            {
                var pobj = new ActPrestamo();
                string Query = ConfigReader.GetQuery( 1, "PRES", "DBQP_SelectPrestamoId");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPrestamo", IdPrestamo);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ActPrestamo obj = MappToPrestamo(reader);
                                pobj = obj;
                            }
                            
                        }
                    }
                }
                return pobj;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_PrestamoId || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetData_PrestamoId || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return null;
            }

        }
        private bool GetExist_PrestamosUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery( 1, "PRES", "DBQP_SelectPrestamoUser");
                int totalPrestamos = 0;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalPrestamos = Convert.ToInt32(reader["TotalPrestamos"]);
                            }
                        }
                    }
                }
                return totalPrestamos > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExist_PrestamosUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetExist_PrestamosUser || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return false;
            }
        }
        private List<DetallesPrestamosUsers> GetData_PrestamosUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery( 1, "PRES", "DBQP_SelectPrestamoUser");

                List<DetallesPrestamosUsers> prestamos = new List<DetallesPrestamosUsers>();
                DetallesPrestamosUsers detallesPrestamos = new DetallesPrestamosUsers();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            decimal valorTotalPrestado = 0;
                            while (reader.Read())
                            {
                                detallesPrestamos.TotalPrestamos = Convert.ToInt32(reader["TotalPrestamos"]);
                                detallesPrestamos.TotalCuotas = Convert.ToInt32(reader["TotalCuota"]);
                                detallesPrestamos.TotalPagoUnico = Convert.ToInt32(reader["TotalPagoUnico"]);
                                detallesPrestamos.TotalAprobado = Convert.ToInt32(reader["TotalAprobado"]);
                                detallesPrestamos.TotalRechazado = Convert.ToInt32(reader["TotalRechazado"]);
                                detallesPrestamos.TotalPendiente = Convert.ToInt32(reader["TotalPendiente"]);
                                DetallesPrestamosUsers.DetallesPorPrestamo prestamo = new DetallesPrestamosUsers.DetallesPorPrestamo
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    Razon = reader["Razon"].ToString(),
                                    Estado = reader["Estado"].ToString()
                                };
                                detallesPrestamos.Detalles.Add(prestamo);
                                valorTotalPrestado += prestamo.Valor;
                            }
                            detallesPrestamos.ValorTotalPrestado = valorTotalPrestado;
                        }
                    }
                }
                prestamos.Add(detallesPrestamos);
                return prestamos;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_PrestamosUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetData_PrestamosUser || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return null;
            }
        }
        private string Auto_GetData_LastIdPres(int IdUser)
        {
            string IdA = string.Empty;
            try
            {
                string Query = ConfigReader.GetQuery( 2, "", "ASQ_SelectLastIdPresUser");
                List<ActAportacione> aportaciones = new List<ActAportacione>();


                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IdA = Convert.ToString(reader["IdPres"]);
                            }
                        }
                    }
                }
                return IdA;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\n Auto_GetData_LastIdPres || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Auto_GetData_LastIdPres || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return null;
            }
        }
        private ActPrestamo MappToPrestamo(MySqlDataReader reader)
        {
            return new ActPrestamo
            {
                Id = Convert.ToInt32(reader["Id"]),
                IdPres = Convert.ToString(reader["IdPres"]),
                IdUser = Convert.ToInt32(reader["IdUser"]),
                Valor = Convert.ToDecimal(reader["Valor"]),
                FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                FechaEntregaDinero = Convert.ToDateTime(reader["FechaEntregaDinero"]),
                FechaInicioPagoCuotas = Convert.ToDateTime(reader["FechaInicioPagoCuotas"]),
                FechaPagoTotalPrestamo = Convert.ToDateTime(reader["FechaPagoTotalPrestamo"]),
                TipoCuota = Convert.ToString(reader["TipoCuota"]),
                Estado = Convert.ToString(reader["Estado"])
            };
        }
        public object OperacionesPrestamos(int Opcion, int Id, int IdUser, string IdPres)
        {
            try
            {
                switch (Opcion)
                {
                    case 1:
                        return GetData_IdPrestamo_IdPres(IdPres);
                    case 2:
                        return GetData_PrestamoId(Id);
                    case 3:
                        return GetExist_PrestamosUser(IdUser);
                    case 4:
                        return GetData_PrestamosUser(IdUser);
                    case 5:
                        return Auto_GetData_LastIdPres(IdUser);
                    default:
                        Console.WriteLine("\n-----------------------------------------");
                        Console.WriteLine("\nOperacionesPrestamos || Opcion Inexistente.");
                        Console.WriteLine("\n-----------------------------------------\n");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nOperacionesPrestamos || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
    }
}
