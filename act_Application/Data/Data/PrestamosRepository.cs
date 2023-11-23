using act_Application.Helper;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Data
{
    public class PrestamosRepository
    {

        public ActPrestamo GetDataPrestamoId(int idPrestamos) //Consulta para obtener todos los datos de una transaccion especifica
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery(1, "SelectPrestamoId");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", idPrestamos);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ActPrestamo transaccion = new ActPrestamo
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    IdPres = Convert.ToString(reader["IdPres"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                                    FechaEntregaDinero = Convert.ToDateTime(reader["FechaEntregaDinero"]),
                                    FechaInicioPagoCuotas = Convert.ToDateTime(reader["FechaIniCoutaPrestamo"]),
                                    FechaPagoTotalPrestamo = Convert.ToDateTime(reader["FechaPagoTotalPrestamo"]),
                                    TipoCuota = Convert.ToString(reader["TipoCuota"]),
                                    Estado = Convert.ToString(reader["Estado"])
                                };
                                return transaccion;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de la transacción");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }

            return null;
        }

        public bool GetExistPrestamosUser(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string prestamosQuery = ConfigReader.GetQuery(1, "SelectPrestamosUser");

            int totalPrestamos = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(prestamosQuery, connection))
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

        public List<DetallesPrestamosUsers> GetDataPrestamosUser(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string prestamosQuery = ConfigReader.GetQuery(1, "SelectPrestamosUser");

            List<DetallesPrestamosUsers> prestamos = new List<DetallesPrestamosUsers>();
            DetallesPrestamosUsers detallesPrestamos = new DetallesPrestamosUsers();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(prestamosQuery, connection))
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
        public string H_GetLastIdPres(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string aportacionesQuery = ConfigReader.GetQuery(2, "SelectLastIdPresUser");
            List<ActAportacione> aportaciones = new List<ActAportacione>();
            string IdA = string.Empty;

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
    }
}
