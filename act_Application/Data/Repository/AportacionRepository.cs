using act_Application.Helper;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class AportacionRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        public bool GetExistAportaciones()
        {
            try
            {
                string aportacionesQuery = ConfigReader.GetQuery(1, "SelectAportaciones");

                int totalAportaciones = 0; // Variable para almacenar el valor de TotalAportaciones

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(aportacionesQuery, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        connection.Open();

                        using (MySqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read()) // Avanzar al primer registro
                            {
                                totalAportaciones = Convert.ToInt32(rd["TotalAportaciones"]);
                            }
                        }
                    }
                }
                // Si totalAportaciones es mayor que 0, devuelve true, de lo contrario, devuelve false.
                return totalAportaciones > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetExistAportaciones | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return false;
            }
        }
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
        public List<ActAportacione> GetDataAportaciones()
        {
            try
            {
                string aportacionesQuery = ConfigReader.GetQuery(1, "SelectAportaciones");

                List<ActAportacione> aportaciones = new List<ActAportacione>();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(aportacionesQuery, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        connection.Open();

                        using (MySqlDataReader rd = cmd.ExecuteReader())
                        {
                            var aportacionesPorUsuario = rd.Cast<IDataRecord>()
                                .Select(r => new
                                {
                                    Id = Convert.ToInt32(r["Id"]),
                                    Razon = Convert.ToString(r["Razon"]),
                                    Valor = Convert.ToDecimal(r["Valor"]),
                                    IdUser = Convert.ToInt32(r["IdUser"]),
                                    FechaAportacion = Convert.ToDateTime(r["FechaAportacion"]),
                                    Estado = Convert.ToString(r["Estado"]),
                                    CapturaPantalla = r.IsDBNull(r.GetOrdinal("CapturaPantalla")) ? null : (byte[])r["CapturaPantalla"],
                                    NombreUsuario = Convert.ToString(r["NombreUsuario"]),

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
            catch (Exception ex)
            {
                Console.WriteLine("GetDataAportaciones | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        public bool GetExistApotacionesUser(int IdUser)
        {
            try
            {
                string aportacionesQuery = ConfigReader.GetQuery(1, "SelectAportacionesUser");

                int totalAportaciones = 0;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(aportacionesQuery, connection))
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
            catch (Exception ex)
            {
                Console.WriteLine("GetExistApotacionesUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return false;
            }
        }
        public List<DetallesAportacionesUsers> GetDataAportacionesUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "SelectAportacionesUser");

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
            catch (Exception ex)
            {
                Console.WriteLine("GetDataAportacionesUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return null;
            }
        }
        public string H_GetLastIdApor (int IdUser)
        {
            string IdA = string.Empty;
            try
            {
                string Query = ConfigReader.GetQuery(2, "SelectLastIdAporUser");
                List<ActAportacione> aportaciones = new List<ActAportacione>();

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
                                IdA = Convert.ToString(reader["IdApor"]);
                            }
                        }
                    }
                }
                return IdA;
            }
            catch (Exception ex)
            {
                Console.WriteLine("H_GetLastIdApor | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return IdA;
            }
        }
    }
}
