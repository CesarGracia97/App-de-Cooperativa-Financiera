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
                                    Destino = reader["Destino"].ToString()
                                };
                                notificaciones.Add(notificacion);
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
    }
}
