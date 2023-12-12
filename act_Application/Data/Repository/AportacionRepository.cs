using act_Application.Helper;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System.Data;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace act_Application.Data.Repository
{
    public class AportacionRepository
    {
        /*
         He desarrollado tantos programas, trantos proyectos, trabajos y ninguno me ha hecho sentir tan antusiasmado como el kue kuiero desarrollar
         contigo a mi lado, uno al kue kuiero poner de nombre "Relacion eterna". Uno donde ambos nos apoyemos, trabajemos, tengamos funciones iterativas
         kue permitan el desarrollo y evolucion mutua como una Inteligencia Artificar de tipo Red Neuronal. Donde podamos crecer y resolver problemas 
         propios y congregados. Donde podamos crecer juntos profesionalmente en tu carrera y yo en la mia como Servidores de Base de Datos enlazados
         Y donde kuiero kue creemos un futuro juntos como, kue fusionemos nuestras pasiones, sueños, deseos, metas, anhelos, fortalezas, debilidades
         miedos, valores y sobre todo nuestros espiritus y creemos algo unico similar al universo en donde tu alma y la mia converjan en armonia 
         como objetos a la espera de una union funcionalmente activa.
         Se que ahora las cosas estan complicadas, y que esto se lo puede ver muy enogorroso que la noche se la ve muy oscura y espeza, pero se que podras ver un bonito
         Amanecer, podremos ver juntos un bonito amanecer. La vida nos pone pruebas todo el tiempo y nuestro primer deber superarlas, y esta es la primera gran prueba
         que tienes y que se que la superaras. Demuestra quien eres, demuestra de lo que estas hecha. Destruye a todo aquel que atente contra ti y crea lo que en tus sueños
         habita. Despues no volveras a ser la misma pero renaceras como un ser nuevo, alguien etereo. Yo creo en eso y no solo volveras a tu Prime... Seras mejor
         que el Prime que obtuviste anteriormente. 
        */
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        private bool GetExist_Aportaciones()
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "APOR", "DBQA_SelectAportaciones");

                int totalAportaciones = 0; // Variable para almacenar el valor de TotalAportaciones

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
                                totalAportaciones = Convert.ToInt32(reader["TotalAportaciones"]);
                            }
                        }
                    }
                }
                // Si totalAportaciones es mayor que 0, devuelve true, de lo contrario, devuelve false.
                return totalAportaciones > 0;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExist_Aportaciones || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetExist_Aportaciones || Error.");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return false;
            }
        }
        private bool GetExist_ApotacionesUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "APOR", "DBQA_SelectAportacionesUser");

                int totalAportaciones = 0;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalAportaciones = Convert.ToInt32(reader["TotalAportaciones"]);
                            }
                        }
                    }
                }
                return totalAportaciones > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExist_ApotacionesUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetExist_ApotacionesUser|| Error.");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return false;
            }
        }
        private List<ActAportacione> GetData_Aportaciones()
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "APOR", "DBQA_SelectAportaciones");

                List<ActAportacione> aportaciones = new List<ActAportacione>();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(Query, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        connection.Open();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            var aportacionesPorUsuario = reader.Cast<IDataRecord>()
                                .Select(reader => new
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    FechaAportacion = Convert.ToDateTime(reader["FechaAportacion"]),
                                    Estado = Convert.ToString(reader["Estado"]),
                                    CapturaPantalla = reader.IsDBNull(reader.GetOrdinal("CapturaPantalla")) ? null : (byte[])reader["CapturaPantalla"],
                                    NombreUsuario = Convert.ToString(reader["NombreUsuario"]),

                                })
                                .ToList();

                            var aportacionesAgrupadas = aportacionesPorUsuario
                                .GroupBy(a => new { a.NombreUsuario, a.FechaAportacion.Month }) // Agrupamos por NombreUsuario y Mes
                                .ToList();

                            foreach (var group in aportacionesAgrupadas)
                            {
                                var aportacion = new ActAportacione
                                {
                                    Id = group.First().Id,
                                    IdUser = group.First().IdUser,
                                    FechaAportacion = group.First().FechaAportacion,
                                    Estado = group.First().Estado,
                                    CapturaPantalla = group.First().CapturaPantalla,
                                    NombreUsuario = group.Key.NombreUsuario

                                };

                                // Calculamos el número de aportaciones y almacenamos detalles
                                aportacion.NumeroAportaciones = group.Count();
                                aportacion.DetallesAportaciones = group.Select(a => new DetalleAportacion
                                {
                                    Valor = (decimal)a.Valor,
                                    FechaAportacion = a.FechaAportacion,
                                    Cuadrante = a.FechaAportacion.Day <= 15 ? 1 : 2
                                }).ToList();

                                // Sumamos los valores para calcular la sumatoria total
                                aportacion.Valor = group.Sum(a => (decimal)a.Valor);

                                aportaciones.Add(aportacion);
                            }
                        }
                    }
                }

                return aportaciones;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_Aportaciones || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetData_Aportaciones || Error.");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return null;
            }
        }
        private List<DetallesAportacionesUsers> GetData_AportacionesUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "APOR", "DBQA_SelectAportacionesUser");

                List<DetallesAportacionesUsers> aportaciones = new List<DetallesAportacionesUsers>();
                DetallesAportacionesUsers detallesAportaciones = new DetallesAportacionesUsers();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            decimal aportacionesAcumuladas = 0;
                            while (reader.Read())
                            {
                                detallesAportaciones.TotalAportaciones = Convert.ToInt32(reader["TotalAportaciones"]);
                                detallesAportaciones.TotalAprobados = Convert.ToInt32(reader["TotalAprobados"]);
                                detallesAportaciones.TotalEspera = Convert.ToInt32(reader["TotalEspera"]);
                                DetallesAportacionesUsers.DetallesPorAportacion aportacion = new DetallesAportacionesUsers.DetallesPorAportacion
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    Aprobacion = Convert.ToString(reader["Aprobacion"]),
                                    Nbanco = Convert.ToString(reader["Nbanco"])
                                };
                                detallesAportaciones.Detalles.Add(aportacion);
                                aportacionesAcumuladas += aportacion.Valor;
                            }
                            detallesAportaciones.AportacionesAcumuladas = aportacionesAcumuladas;
                        }
                    }
                }

                aportaciones.Add(detallesAportaciones);
                return aportaciones;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_AportacionesUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetData_AportacionesUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        private string Auto_GetData_LastIdApor(int IdUser)
        {
            string IdA = string.Empty;
            try
            {
                string Query = ConfigReader.GetQuery(2, "", "ASQ_SelectLastIdAporUser");
                List<ActAportacione> aportaciones = new List<ActAportacione>();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", IdUser);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IdA = Convert.ToString(reader["IdApor"]);
                            }
                        }
                    }
                }
                return IdA;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nH_GetData_LastIdApor || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Auto_GetData_LastIdApor | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return IdA;
            }
        }
        private int GetData_IdAportacion_IdApor(string IdApor) //Obtienes el Id de una aportacion por medio de su IdPersonalizado.
        {
            try
            {
                int Id = 0;
                string Query = ConfigReader.GetQuery(1, "APOR", "DBQA_SelectAportacionesIdApor");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@IdApor", IdApor);
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
                Console.WriteLine($"\nGetData_IdAportacion || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGetData_IdAportacion | Error.");
                Console.WriteLine("\nDetalles del error: " + ex.Message);
                return -1;
            }
        }
        private ActAportacione GetData_DataAportaciones_Id(int Id) //Obtienes todos los datos de una aportacion por medio de su Id de Registro.
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "APOR", "DBQA_SelectAportacionesDataId");
                ActAportacione aobj = new ActAportacione();
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActAportacione obj = MapToAportaciones(reader);
                            aobj = obj;
                        }
                    }
                }
                return aobj;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_DataAportaciones_Id || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGetData_DataAportaciones_Id | Error.");
                Console.WriteLine("\nDetalles del error: " + ex.Message);
                return null;
            }
        }
        private ActAportacione MapToAportaciones (MySqlDataReader reader)
        {
            return new ActAportacione
            {
                Id = Convert.ToInt32(reader["Id"]),
                IdApor = Convert.ToString(reader["IdApor"]),
                IdUser = Convert.ToInt32(reader["IdUser"]),
                FechaAportacion = Convert.ToDateTime(reader["FechaAportacion"]),
                Cuadrante = Convert.ToString(reader["Cuadrante"]),
                Valor = Convert.ToDecimal(reader["Valor"]),
                NBancoOrigen = Convert.ToString(reader["NBancoOrigen"]),
                CBancoOrigen = Convert.ToString(reader["CBancoOrigen"]),
                NBancoDestino = Convert.ToString(reader["NBancoDestino"]),
                CBancoDestino = Convert.ToString(reader["CBancoDestino"]),
                CapturaPantalla = reader.IsDBNull(reader.GetOrdinal("CapturaPantalla")) ? null : (byte[])reader["CapturaPantalla"],
                Estado = Convert.ToString(reader["Estado"])
            };
        }
        public object OperacionesAportaciones (int Opciones, int Id, int IdUser, string Cadena)
        {
            try
            {
                switch (Opciones)
                {
                    case 1:
                        return GetExist_Aportaciones();
                    case 2:
                        return GetData_Aportaciones();
                    case 3:
                        return GetExist_ApotacionesUser(IdUser);
                    case 4:
                        return GetData_AportacionesUser(IdUser);
                    case 5:
                        return Auto_GetData_LastIdApor(IdUser);
                    case 6:
                        return GetData_IdAportacion_IdApor(Cadena);
                    case 7:
                        return GetData_DataAportaciones_Id(Id);
                    default:
                        Console.WriteLine("\n----------------------------------------------");
                        Console.WriteLine("\nOperacionesAportaciones || Opcion Inexistente.");
                        Console.WriteLine("\n----------------------------------------------\n");
                        return null;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nOperacionesAportaciones || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
    }
}
