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
                string query = ConfigReader.GetQuery("SelectPrestamoId");
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
                                    Razon = Convert.ToString(reader["Razon"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    Estado = Convert.ToString(reader["Estado"]),
                                    FechaEntregaDinero = Convert.ToDateTime(reader["FechaEntregaDinero"]),
                                    FechaPagoTotalPrestamo = Convert.ToDateTime(reader["FechaPagoTotalPrestamo"]),
                                    FechaIniCoutaPrestamo = Convert.ToDateTime(reader["FechaIniCoutaPrestamo"]),
                                    TipoCuota = Convert.ToString(reader["TipoCuota"]),
                                    IdParticipantes = Convert.ToInt32(reader["IdParticipantes"]),
                                    FechaGeneracion = Convert.ToDateTime(reader["FechaEntregaDinero"])
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
            string prestamosQuery = ConfigReader.GetQuery("SelectPrestamosUser");

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
            string prestamosQuery = ConfigReader.GetQuery("SelectPrestamosUser");

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

    }
}
