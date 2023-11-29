using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class EventosRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        public bool GetExistEventos()
        {
            try
            {
                string Query = ConfigReader.GetQuery1( 1, "EVEN", "DBQE_SelectEventos");

                int totalAportaciones = 0; // Variable para almacenar el valor de TotalAportaciones

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(Query, connection))
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
            catch(Exception ex)
            {
                Console.WriteLine("GetExistEventos | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return false;
            }
        }
        public List<ActEvento> GetAllDataEventos()
        {
            List<ActEvento> eventoList = new List<ActEvento>();
            try
            {
                string Query = ConfigReader.GetQuery1( 1, "EVEN", "DBQE_SelectEventos");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(Query, connection))

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        /*
                         He desarrollado tantos programas, trantos proyectos, trabajos y ninguno me ha hecho sentir tan antusiasmado como el kue kuiero desarrollar
                         contigo a mi lado, uno al kue kuiero poner de nombre "Relacion eterna". Uno donde ambos nos apoyemos, trabajemos, tengamos funciones iterativas
                         kue permitan el desarrollo y evolucion mutua como una Inteligencia Artificar de tipo Red Neuronal. Donde podamos crecer y resolver problemas 
                         propios y congregados. Donde podamos crecer juntos profesionalmente en tu carrera y yo en la mia como Servidores de Base de Datos enlazados
                         Y donde kuiero kue creemos un futuro juntos como, kue fusionemos nuestras pasiones, sueños, deseos, metas, anhelos, fortalezas, debilidades
                         miedos, valores y sobre todo nuestros espiritus y creemos algo unico similar al universo en donde tu alma y la mia converjan en armonia 
                         como objetos a la espera de una union funcionalmente activa.
                        */
                        while (reader.Read())
                        {
                            ActEvento obj = MapToActEventos(reader);
                            eventoList.Add(obj);
                            var prestamos = new PrestamosRepository();
                            prestamos.GetDataPrestamoId(obj.IdPrestamo);
                        }
                    }
                }
                return eventoList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllDataEventos | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        public ActEvento GetDataEventoPorId(int Id)
        {
            try
            {
                string Query = ConfigReader.GetQuery1( 1, "EVEN", "DBQE_SelectEventosUser");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ActEvento eventos = MapToActEventos(reader);
                                return eventos;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDataEventoPorId | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        public ActEvento Auto_GetParticipantesPrestamoEvento(int IdPrestamo)
        {
            try
            {
                string Query = ConfigReader.GetQuery1( 3, "", "ATQ_SelectParticipantesPrestamoEvento");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPrestamo", IdPrestamo);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ActEvento obj = MapToActEventos(reader);
                                return obj;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Auto_GetParticipantesPrestam | Error");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        private ActEvento MapToActEventos(MySqlDataReader reader)
        {
            return new ActEvento 
            {
                Id = Convert.ToInt32(reader["Id"]),
                IdEven = Convert.ToString(reader["IdEven"]),
                IdPrestamo = Convert.ToInt32(reader["IdPrestamo"]),
                IdUser = Convert.ToInt32(reader["IdUser"]),
                ParticipantesId = Convert.ToString(reader["ParticipantesId"]),
                NombresPId = Convert.ToString(reader["NombresPId"]),
                FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                FechaFinalizacion = Convert.ToDateTime(reader["FechaFinalizacion"]),
                Estado = Convert.ToString(reader["Estado"]),
                NombreUsuario = Convert.ToString(reader["NombreUsuario"])
            };
        }
    }
}
