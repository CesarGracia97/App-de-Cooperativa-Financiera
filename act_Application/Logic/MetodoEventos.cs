using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;

    namespace act_Application.Logic
    {
        public class MetodoEventos
        {
            public IEnumerable<ActParticipante> GetDataEventos() //Consulta para obtener todos los datos de los eventos de Participacion
            {
                string connectionString = AppSettingsHelper.GetConnectionString();
                try
                {
                    string query = ConfigReader.GetQuery("SelectDatosEventoParticipante");
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                List<ActParticipante> eventos = new List<ActParticipante>();
                                while (reader.Read())
                                {
                                    ActParticipante eve = new ActParticipante
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        IdTransaccion = Convert.ToInt32(reader["IdTransaccion"]),
                                        FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                        FechaFinalizacion = Convert.ToDateTime(reader["FechaFinalizacion"]),
                                        FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                                        ParticipantesId = reader["Destino"].ToString(),
                                        ParticipantesNombre = reader["ParticipantesNombre"].ToString(),
                                        Estado = reader["Estado"].ToString(),
                                    };
                                    eventos.Add(eve);
                                }
                                return eventos;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hubo un error en la consulta y en la optencion de los datos del evento");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                    return null;
                }
            }

        }
    }
