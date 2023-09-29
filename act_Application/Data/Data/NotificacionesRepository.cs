using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace act_Application.Data.Data
{
    public class NotificacionesRepository
    {

        public int TotalNotificacionesAdmin { get; set; }
        public int TotalNotificacionesUser { get; set; }
        public List<ActNotificacione> Notificaciones { get; set; }

        public NotificacionesRepository GetDataNotitficacionesAdmin()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            NotificacionesRepository result = new NotificacionesRepository();
            try
            {
                string query = ConfigReader.GetQuery("");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.TotalNotificacionesAdmin = Convert.ToInt32(reader["TotalNotificaciones"]);
                            List<ActNotificacione> multas = new List<ActNotificacione>();

                            do
                            {
                                ActNotificacione mul = new ActNotificacione
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
                                multas.Add(mul);

                            } while (reader.Read());

                            result.Notificaciones = multas;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de los datos de notificacion del Administrador.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                result.TotalNotificacionesAdmin = -1; // Valor negativo para indicar un error
                result.Notificaciones = null;
            }
            return result;
        }


        public NotificacionesRepository GetDataNotificacionesUser(int userId)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            NotificacionesRepository result = new NotificacionesRepository();
            try
            {
                string query = ConfigReader.GetQuery("");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        command.Parameters.AddWithValue("@Id", userId);
                        if (reader.Read())
                        {
                            result.TotalNotificacionesAdmin = Convert.ToInt32(reader["TotalNotificaciones"]);
                            List<ActNotificacione> multas = new List<ActNotificacione>();

                            do
                            {
                                ActNotificacione mul = new ActNotificacione
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
                                multas.Add(mul);

                            } while (reader.Read());

                            result.Notificaciones = multas;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo En la consulta de las Notificaciones del Usuario");
                Console.WriteLine("Detalles del error: " + ex.Message);
                result.TotalNotificacionesAdmin = -1; // Valor negativo para indicar un error
                result.Notificaciones = null;
            }
            return result;
        }


        public int GetTotalNotificacionesAdministrador()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectExistenciaNotifiAdmin");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo En la consulta de la Notificacion del Administrador");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return -1;
            }
        }
        
        public int GetTotalNotificacionesUsuario(int userId)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectExistenciaNotifiUsuario");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", userId); // Pasar el ID del usuario como parámetro
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de notificaciones del usuario");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return -1;
            }
        }
    }
}
