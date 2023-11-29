using act_Application.Helper;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Repository
{
    public class PrestamosRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        public ActPrestamo GetDataPrestamoId(string IdPres) //Consulta para obtener todos los datos de una transaccion especifica
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "SelectPrestamoId");
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
                                ActPrestamo obj = MappToPrestamo(reader);
                                return obj;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDataPrestamoId | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        public bool GetExistPrestamosUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "SelectPrestamosUser");

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
            catch (Exception ex)
            {
                Console.WriteLine("GetExistPrestamosUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return false;
            }
        }
        public List<DetallesPrestamosUsers> GetDataPrestamosUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "SelectPrestamosUser");

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
            catch (Exception ex)
            {
                Console.WriteLine("GetDataPrestamosUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        public string H_GetLastIdPres(int IdUser)
        {
            string IdA = string.Empty;
            try
            {
                string aportacionesQuery = ConfigReader.GetQuery(2, "SelectLastIdPresUser");
                List<ActAportacione> aportaciones = new List<ActAportacione>();


                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(aportacionesQuery, connection))
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
            catch (Exception ex)
            {
                Console.WriteLine("H_GetLastIdPres | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return IdA;
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
    }
}
