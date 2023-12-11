using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class EventosRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        private bool GetExist_Eventos()
        {
            try
            {
                string Query = ConfigReader.GetQuery( 1, "EVEN", "DBQE_SelectEventos");

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
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExist_Eventos || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetExist_Eventos | Error.");
                Console.WriteLine($"\nDetalles del error:  {ex.Message}\n");
                return false;
            }
        }
        private List<ActEvento> GetData_Eventos()
        {
            List<ActEvento> eventoList = new List<ActEvento>();
            try
            {
                string Query = ConfigReader.GetQuery( 1, "EVEN", "DBQE_SelectEventos");

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
                            ActEvento obj = MapToEventos(reader);
                            eventoList.Add(obj);
                            var prestamos = new PrestamosRepository();
                            prestamos.OperacionesPrestamos( 2, obj.IdPrestamo, 0, "");
                        }
                    }
                }
                return eventoList;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_Eventos || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetData_Eventos | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        private ActEvento GetData_EventosId(int Id)
        {
            try
            {
                string Query = ConfigReader.GetQuery( 1, "EVEN", "DBQE_SelectEventosUser");
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
                                ActEvento eventos = MapToEventos(reader);
                                return eventos;
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_EventosId || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetData_EventosId | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        private ActEvento Auto_GetData_ParticipantesEventos(int IdPrestamo)
        {
            try
            {
                string Query = ConfigReader.GetQuery( 3, "", "ATQ_SelectParticipantesPrestamoEvento");
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
                                ActEvento obj = MapToEventos(reader);
                                return obj;
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nAuto_GetData_ParticipantesEventos || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Auto_GetData_ParticipantesEventos | Error");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        private int GetData_IdEvento(string IdEven) //Obtener el Id de Registro de Evento por medio de su Id Personalizado.
        {
            try
            {
                int Id = 0;
                string Query = ConfigReader.GetQuery(1, "MULT", "DBQE_SelectEventosIdEven");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdEvent", IdEven);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = Convert.ToInt32(reader["Id"]);
                            }
                        }
                    }
                }
                return Id;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_IdEvento || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGetData_IdEvento | Error.");
                Console.WriteLine("\nDetalles del error: " + ex.Message);
                return -1;
            }
        } 
        private ActEvento MapToEventos(MySqlDataReader reader)
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
        public object OperacionesEventos(int Opcion, int Id, int IdUser, string Cadena)
        {
            try
            {
                switch (Opcion)
                {
                    case 1:
                        return GetExist_Eventos();
                    case 2:
                        return GetData_Eventos();
                    case 3:
                        return GetData_EventosId(Id);
                    case 4:
                        return Auto_GetData_ParticipantesEventos(Id);
                    case 5:
                        return GetData_IdEvento(Cadena);
                    default:
                        Console.WriteLine("\n-----------------------------------------");
                        Console.WriteLine("\nOperacionesEventos || Opcion Inexistente.");
                        Console.WriteLine("\n-----------------------------------------\n");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nOperacionesEventos || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
    }
}
