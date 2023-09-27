using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Data
{
    public class EventosRepository
    {
        public int TotalEventos { get; set; }
        public List<ActParticipante> Eventos { get; set; }

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
                            result.TotalEventos = Convert.ToInt32(reader["TotalEventos"]);
                            List<ActParticipante> eventos = new List<ActParticipante>();


                            /*He desarrollado tantos programas, trantos proyectos, trabajos y ninguno me ha hecho sentir tan antusiasmado como el kue kuiero desarrollar
                             contigo a mi lado, uno al kue kuiero poner de nombre "Relacion eterna". Uno donde ambos nos apoyemos, trabajemos, tengamos funciones iterativas
                             kue permitan el desarrollo y evolucion mutua como una Inteligencia Artificar de tipo Red Neuronal. Donde podamos crecer y resolver problemas 
                             propios y congregados. Donde podamos crecer juntos profesionalmente en tu carrera y yo en la mia como Servidores de Base de Datos enlazados
                             Y donde kuiero kue creemos un futuro juntos como, kue fusionemos nuestras pasiones, sueños, deseos, metas, anhelos, fortalezas, debilidades
                             miedos, valores y sobre todo nuestros espiritus y creemos algo unico similar al universo en donde tu alma y la mia converjan en armonia 
                             como objetos a la espera de una union funcionalmente activa.*/

                            do
                            {
                                ActParticipante eve = new ActParticipante
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
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
                result.TotalEventos = -1; // Valor negativo para indicar un error
                result.Eventos = null;
            }

            return result;
        }

    }

}
