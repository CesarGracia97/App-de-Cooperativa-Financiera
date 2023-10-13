using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Data;

namespace act_Application.Data.Data
{
    public class EventosRepository
    {
        public List<ActEvento> Eventos { get; set; }

        public bool GetExistEventos()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string eventosQuery = ConfigReader.GetQuery("SelectEvents");

            int totalAportaciones = 0; // Variable para almacenar el valor de TotalAportaciones

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(eventosQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read()) // Avanzar al primer registro
                        {
                            totalAportaciones = Convert.ToInt32(rd["TotalEventos"]);
                        }
                    }
                }
            }
            // Si totalAportaciones es mayor que 0, devuelve true, de lo contrario, devuelve false.
            return totalAportaciones > 0;
        }

        public EventosRepository GetDataEventos()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            EventosRepository result = new EventosRepository();
            try
            {
                string query = ConfigReader.GetQuery("SelectEvents");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            List<ActEvento> eventos = new List<ActEvento>();
                            /*He desarrollado tantos programas, trantos proyectos, trabajos y ninguno me ha hecho sentir tan antusiasmado como el kue kuiero desarrollar
                             contigo a mi lado, uno al kue kuiero poner de nombre "Relacion eterna". Uno donde ambos nos apoyemos, trabajemos, tengamos funciones iterativas
                             kue permitan el desarrollo y evolucion mutua como una Inteligencia Artificar de tipo Red Neuronal. Donde podamos crecer y resolver problemas 
                             propios y congregados. Donde podamos crecer juntos profesionalmente en tu carrera y yo en la mia como Servidores de Base de Datos enlazados
                             Y donde kuiero kue creemos un futuro juntos como, kue fusionemos nuestras pasiones, sueños, deseos, metas, anhelos, fortalezas, debilidades
                             miedos, valores y sobre todo nuestros espiritus y creemos algo unico similar al universo en donde tu alma y la mia converjan en armonia 
                             como objetos a la espera de una union funcionalmente activa.*/
                            do
                            {
                                ActEvento eve = new ActEvento
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    IdTransaccion = Convert.ToInt32(reader["IdTransaccion"]),
                                    FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                    FechaFinalizacion = Convert.ToDateTime(reader["FechaFinalizacion"]),
                                    FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                                    ParticipantesId = reader["ParticipantesId"].ToString(),
                                    ParticipantesNombre = reader["ParticipantesNombre"].ToString(),
                                    Estado = reader["Estado"].ToString(),
                                    NombreUsuario = reader["NombreUsuario"].ToString()
                                };
                                eventos.Add(eve);
                                TransaccionesRepository transacciones = new TransaccionesRepository();
                                transacciones.GetDataTransaccionId(eve.IdTransaccion);

                            } while (reader.Read());

                            result.Eventos = eventos;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de eventos.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                result.Eventos = null;
            }

            return result;
        }

        public ActEvento GetDataEventoPorId(int Id)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelectEventosUser");
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
