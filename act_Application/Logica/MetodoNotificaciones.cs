using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;

namespace act_Application.Logica
{
    public class MetodoNotificaciones
    {
        public IEnumerable<ActNotificacione> GetNotificacionesAdministrador()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectNotifiAdmin"); // Consulta para obtener las notificaciones del administrador
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<ActNotificacione> notificaciones = new List<ActNotificacione>();
                            while (reader.Read())
                            {
                                ActNotificacione notificacion = new ActNotificacione
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    Razon = reader["Razon"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString(),
                                    FechaNotificacion = Convert.ToDateTime(reader["FechaNotificacion"]),
                                    Destino = reader["Destino"].ToString(),
                                    IdTransacciones = Convert.ToInt32(reader["IdTransacciones"]),
                                    IdAportaciones = Convert.ToInt32(reader["IdAportaciones"]),
                                    IdCuotas = Convert.ToInt32(reader["IdCuotas"])
                                };
                                notificaciones.Add(notificacion);
                                GetTransaccionPorId(notificacion.IdTransacciones);
                            }
                            return notificaciones;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de notificaciones");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
    
        public ActTransaccione GetTransaccionPorId(int idTransacciones)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelecInfoTransacciones");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdTransacciones", idTransacciones);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ActTransaccione transaccion = new ActTransaccione
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Razon = reader["Razon"].ToString(),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    Estado = reader["Estado"].ToString(),
                                    FechaEntregaDinero = Convert.ToDateTime(reader["FechaEntregaDinero"]),
                                    FechaPagoTotalPrestamo = Convert.ToDateTime(reader["FechaPagoTotalPrestamo"]),
                                    FechaIniCoutaPrestamo = Convert.ToDateTime(reader["FechaIniCoutaPrestamo"]),
                                    TipoCuota = reader["TipoCuota"].ToString()
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
    }
}
