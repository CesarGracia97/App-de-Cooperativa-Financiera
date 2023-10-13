using act_Application.Helper;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Data
{
    public class TransaccionesRepository
    {

        public ActTransaccione GetDataTransaccionId(int idTransacciones) //Consulta para obtener todos los datos de una transaccion especifica
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectTransaccionId");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", idTransacciones);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ActTransaccione transaccion = new ActTransaccione
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

        public bool GetExistTransaccionesUser(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string transaccionesQuery = ConfigReader.GetQuery("SelectTransaccionesUser");

            int totalTransacciones = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(transaccionesQuery, connection))
                {
                    command.Parameters.AddWithValue("@IdUser", IdUser);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalTransacciones = Convert.ToInt32(reader["TotalTransacciones"]);
                        }
                    }
                }
            }
            return totalTransacciones > 0;
        }

        public List<DetallesTransaccionesUsers> GetDataTransaccionesUser(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string transaccionesQuery = ConfigReader.GetQuery("SelectTransaccionesUser");

            List<DetallesTransaccionesUsers> transacciones = new List<DetallesTransaccionesUsers>();
            DetallesTransaccionesUsers detallesTransacciones = new DetallesTransaccionesUsers();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(transaccionesQuery, connection))
                {
                    command.Parameters.AddWithValue("@IdUser", IdUser);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        decimal valorTotalPrestado = 0;
                        while (reader.Read())
                        {
                            detallesTransacciones.TotalTransacciones = Convert.ToInt32(reader["TotalTransacciones"]);
                            detallesTransacciones.TotalCuotas = Convert.ToInt32(reader["TotalCuota"]);
                            detallesTransacciones.TotalPagoUnico = Convert.ToInt32(reader["TotalPagoUnico"]);
                            detallesTransacciones.TotalAprobado = Convert.ToInt32(reader["TotalAprobado"]);
                            detallesTransacciones.TotalRechazado = Convert.ToInt32(reader["TotalRechazado"]);
                            DetallesTransaccionesUsers.DetallesPorTransaccion transaccion = new DetallesTransaccionesUsers.DetallesPorTransaccion
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Valor = Convert.ToDecimal(reader["Valor"]),
                                Razon = reader["Razon"].ToString(),
                                Estado = reader["Estado"].ToString()
                            };
                            detallesTransacciones.Detalles.Add(transaccion);
                            valorTotalPrestado += transaccion.Valor;
                        }
                        detallesTransacciones.ValorTotalPrestado = valorTotalPrestado;
                    }
                }
            }
            transacciones.Add(detallesTransacciones);
            return transacciones;
        }

    }
}
