using act_Application.Data.Data;
using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;

namespace act_Application.Logic
{
    public class MetodoNotificaciones
    {
        public IEnumerable<ActNotificacione> GetNotificacionesAdministrador() //Consulta para obtener todos los datos de las notificacionesAdmin del administrador
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectNotifiAdmin");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<ActNotificacione> notificacionesAdmin = new List<ActNotificacione>();
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
                                notificacionesAdmin.Add(notificacion);
                                TransaccionesRepository transacciones = new TransaccionesRepository();
                                transacciones.GetTransaccionPorId(notificacion.IdTransacciones);
                            }
                            return notificacionesAdmin;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de notificacionesUser");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }

        public IEnumerable<ActNotificacione> GetNotificacionesUsuario(int userId) //Consulta para obtener todos los datos de las notificacionesUser del administrador
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectNotifiUser");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", userId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            List<ActNotificacione> notificacionesUser = new List<ActNotificacione>();
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
                                    IdCuotas = Convert.ToInt32(reader["IdCuotas"]),
                                    
                                };
                                notificacionesUser.Add(notificacion);
                                TransaccionesRepository transaccion = new TransaccionesRepository();
                                transaccion.GetTransaccionPorId(notificacion.IdTransacciones);
                            }
                            return notificacionesUser;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de notificacionesUser");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }

        public ActEvento GetRegistroParticipante (int Id)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectParticipantes");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ActEvento transaccion = new ActEvento
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                    FechaFinalizacion = Convert.ToDateTime(reader["FechaFinalizacion"]),
                                    FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                                    ParticipantesId = reader["ParticipantesId"].ToString(),
                                    ParticipantesNombre = reader["ParticipantesNombre"].ToString(),
                                    Estado = reader["Estado"].ToString(),
                                    IdTransaccion = Convert.ToInt32(reader["IdTransaccion"])
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
