using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class NotificacionesRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        public bool GetExistNotificacionesAdmin()
        {
            string Query = ConfigReader.GetQuery( 1, "SelectAdminNotificacion");
            int TotalN = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(Query, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Avanzar al primer registro
                        {
                            TotalN = Convert.ToInt32(reader["TotalNotificaciones"]);
                        }
                    }
                }
            }
            return TotalN > 0;
        }
        public bool GetExistNotificacionesUser(int userId)
        {
            string Query = ConfigReader.GetQuery( 1, "SelectUserNotificacion");
            int TotalN = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(Query, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", userId);
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Avanzar al primer registro
                        {
                            TotalN = Convert.ToInt32(reader["TotalNotificaciones"]);
                        }
                    }
                }
            }
            return TotalN > 0;
        }
        public List<ActNotificacione> GetDataNotificacionesAdmin() //Consulta para obtener todos los datos de las notificacionesAdmin del administrador
        {
            try
            {
                List<ActNotificacione> notifiAdmin = new List<ActNotificacione>();
                string Query = ConfigReader.GetQuery( 1, "SelectAdminNotificacion");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ActNotificacione obj = MapToNotificaciones(reader);
                                notifiAdmin.Add(obj);
                                PrestamosRepository prestamos = new PrestamosRepository();
                                prestamos.GetDataPrestamoId(obj.IdActividad);
                            }
                        }
                    }
                }
                return notifiAdmin;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de notifiUser");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        public List<ActNotificacione> GetDataNotificacionesUser(int userId) //Consulta para obtener todos los datos de las notificacionesUser del administrador
        {
            try
            {
                List<ActNotificacione> notifiUser = new List<ActNotificacione>();
                string Query = ConfigReader.GetQuery( 1, "SelectUserNotificacion");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", userId);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        { 
                            while (reader.Read())
                            {
                                ActNotificacione obj = MapToNotificaciones(reader);
                                
                                notifiUser.Add(obj);
                                PrestamosRepository prestamo = new PrestamosRepository();
                                prestamo.GetDataPrestamoId(obj.IdActividad);
                            }
                        }
                    }
                }
                return notifiUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de notifiUser");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        private ActNotificacione MapToNotificaciones(MySqlDataReader reader)
        {
            return new ActNotificacione
            {
                Id = Convert.ToInt32(reader["Id"]),
                IdUser = Convert.ToInt32(reader["IdUser"]),
                IdActividad = Convert.ToString(reader["IdActividad"]),
                FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                Razon = Convert.ToString(reader["Razon"]),
                Descripcion = Convert.ToString(reader["Descripcion"]),
                Destino = Convert.ToString(reader["Destino"]),
                Visto = Convert.ToString(reader["Visto"])
            };
        }
    }
}
