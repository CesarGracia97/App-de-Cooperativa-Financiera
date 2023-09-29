using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data
{
    public class AportacionRepository
    {
        public int TotalAportaciones { get; set; }
        public List <ActAportacione> Aportes { get; set; }

        public AportacionRepository GetDataAportaciones()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            AportacionRepository result = new AportacionRepository();
            try
            {
                string query = ConfigReader.GetQuery("SelectAportaciones");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.TotalAportaciones = Convert.ToInt32(reader["TotalAportaciones"]);
                            List<ActAportacione> aportes = new List<ActAportacione>();

                            // Crear un diccionario para agrupar las aportaciones por Nombre y Mes
                            Dictionary<string, Dictionary<string, List<ActAportacione>>> groupedAportaciones =
                                new Dictionary<string, Dictionary<string, List<ActAportacione>>>();

                            do
                            {
                                ActAportacione apo = new ActAportacione
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    Razon = reader["Razon"].ToString(),
                                    Aprobacion = reader["Aprobacion"].ToString(),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    FechaAportacion = Convert.ToDateTime(reader["FechaAportacion"]),
                                    //CapturaPantalla = Convert.IsDBNull(reader.GetOrdinal("CapturaPantalla")) ? null : (byte[])reader["CapturaPantalla"],
                                    NombreUsuario = reader["NombreUsuario"].ToString()
                                };
                                aportes.Add(apo);

                                // Agrupar las aportaciones por Nombre y Mes
                                string nombreUsuario = apo.NombreUsuario;
                                string mes = apo.FechaAportacion.ToString("MMMM yyyy"); // Puedes personalizar el formato de la fecha
                                if (!groupedAportaciones.ContainsKey(nombreUsuario))
                                {
                                    groupedAportaciones[nombreUsuario] = new Dictionary<string, List<ActAportacione>>();
                                }
                                if (!groupedAportaciones[nombreUsuario].ContainsKey(mes))
                                {
                                    groupedAportaciones[nombreUsuario][mes] = new List<ActAportacione>();
                                }
                                groupedAportaciones[nombreUsuario][mes].Add(apo);
                            } while (reader.Read());

                            // Ahora puedes procesar las aportaciones agrupadas
                            foreach (var usuarioKvp in groupedAportaciones)
                            {
                                var nombreUsuario = usuarioKvp.Key;
                                foreach (var mesKvp in usuarioKvp.Value)
                                {
                                    var mes = mesKvp.Key;
                                    var aportacionesMes = mesKvp.Value;

                                    // Crea un objeto ActAportacione para el resultado
                                    var aportacion = new ActAportacione
                                    {
                                        NombreUsuario = nombreUsuario,
                                        FechaAportacion = DateTime.Parse(mes), // Convierte el mes de nuevo a DateTime
                                        NumeroAportaciones = aportacionesMes.Count,
                                        DetallesAportaciones = aportacionesMes.Select(a => new DetalleAportacion
                                        {
                                            Valor = a.Valor,
                                            FechaAportacion = a.FechaAportacion,
                                            Cuadrante = a.FechaAportacion.Day <= 15 ? 1 : 2
                                        }).ToList(),
                                        Valor = aportacionesMes.Sum(a => a.Valor)
                                    };

                                    aportes.Add(aportacion);
                                }
                            }

                            result.Aportes = aportes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de las aportaciones.");
                Console.WriteLine("Detalles del error: " + ex.Message);
                result.TotalAportaciones = -1; // Valor negativo para indicar un error
                result.Aportes = null;
            }
            return result;
        }
        public List<ActCuentaDestino> ObtenerElementos()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string DestinoQuery = ConfigReader.GetQuery("SelectDestino");

            List<ActCuentaDestino> destinos = new List<ActCuentaDestino>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(DestinoQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        var DestinoElement = rd.Cast<IDataRecord>().Select(r => new
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            Nombre = r["Razon"].ToString(),
                            Valor = Convert.ToSingle(r["Valor"])
                        }).ToList();
                    }
                }
            }

            return destinos;
        }
    }
}
