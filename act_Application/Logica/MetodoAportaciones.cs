using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;


namespace act_Application.Logica
{
    public class MetodoAportaciones
    {
        public List<ActAportacione> ObtenerAportaciones()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string aportacionesQuery = ConfigReader.GetQuery("SelectAportantes");

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
                                Razon = r["Razon"].ToString(),
                                Valor = Convert.ToSingle(r["Valor"]),
                                IdUser = Convert.ToInt32(r["IdUser"]),
                                FechaAportacion = Convert.ToDateTime(r["FechaAportacion"]),
                                Aprobacion = r["Aprobacion"].ToString(),
                                CapturaPantalla = r.IsDBNull(r.GetOrdinal("CapturaPantalla")) ? null : (byte[])r["CapturaPantalla"],
                                NombreUsuario = r["NombreUsuario"].ToString()

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
                                Razon = group.First().Razon,
                                IdUser = group.First().IdUser,
                                FechaAportacion = group.First().FechaAportacion,
                                Aprobacion = group.First().Aprobacion,
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
    }
}
