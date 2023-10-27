using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Claims;

namespace act_Application.Data.Data
{
    public class NotificacionesRepository
    {

        public bool GetExistNotificacionesAdmin()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string notificacionesQuery = ConfigReader.GetQuery("SelectAdminNotificacion");
            int totalNotificaciones = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(notificacionesQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read()) // Avanzar al primer registro
                        {
                            totalNotificaciones = Convert.ToInt32(rd["TotalNotificaciones"]);
                        }
                    }
                }
            }
            return totalNotificaciones > 0;
        }

        public bool GetExistNotificacionesUser(int userId)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string notificacionesQuery = ConfigReader.GetQuery("SelectUserNotificacion");
            int totalNotificaciones = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(notificacionesQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", userId);
                    connection.Open();
                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read()) // Avanzar al primer registro
                        {
                            totalNotificaciones = Convert.ToInt32(rd["TotalNotificaciones"]);
                        }
                    }
                }
            }
            return totalNotificaciones > 0;
        }

        public IEnumerable<ActNotificacione> GetDataNotificacionesAdmin() //Consulta para obtener todos los datos de las notificacionesAdmin del administrador
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectAdminNotificacion");
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
                                    Razon = Convert.ToString(reader["Razon"]),
                                    Descripcion = Convert.ToString(reader["Descripcion"]),
                                    FechaNotificacion = Convert.ToDateTime(reader["FechaNotificacion"]),
                                    Destino = Convert.ToString(reader["Destino"]),
                                    IdPrestamo = Convert.ToInt32(reader["IdPrestamo"]),
                                    IdAportaciones = Convert.ToInt32(reader["IdAportaciones"]),
                                    IdCuotas = Convert.ToInt32(reader["IdCuotas"])
                                };
                                notificacionesAdmin.Add(notificacion);
                                PrestamosRepository prestamos = new PrestamosRepository();
                                prestamos.GetDataPrestamoId(notificacion.IdPrestamo);
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

        public IEnumerable<ActNotificacione> GetDataNotificacionesUser(int userId) //Consulta para obtener todos los datos de las notificacionesUser del administrador
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectUserNotificacion");
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
                                    Razon = Convert.ToString(reader["Razon"]),
                                    Descripcion = Convert.ToString(reader["Descripcion"]),
                                    FechaNotificacion = Convert.ToDateTime(reader["FechaNotificacion"]),
                                    Destino = Convert.ToString(reader["Destino"]),
                                    IdPrestamo = Convert.ToInt32(reader["IdPrestamo"]),
                                    IdAportaciones = Convert.ToInt32(reader["IdAportaciones"]),
                                    IdCuotas = Convert.ToInt32(reader["IdCuotas"]),

                                };
                                notificacionesUser.Add(notificacion);
                                PrestamosRepository prestamo = new PrestamosRepository();
                                prestamo.GetDataPrestamoId(notificacion.IdPrestamo);
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
    
    }
}
