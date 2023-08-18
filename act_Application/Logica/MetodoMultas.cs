using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Logica
{
    public class MetodoMultas
    {
        public List<ActMulta> ObtenerMultas()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string multasQuery = ConfigReader.GetQuery("SelectMultados");

            List<ActMulta> multas = new List<ActMulta>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(multasQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        var multasPorUsuario = rd.Cast<IDataRecord>()
                            .Select(r => new
                            {
                                Id = Convert.ToInt32(r["Id"]),
                                IdUser = Convert.ToInt32(r["IdUser"]),
                                Valor = Convert.ToDecimal(r["Valor"]),
                                FechaMulta = Convert.ToDateTime(r["FechaMulta"]),
                                NombreUsuario = r["NombreUsuario"].ToString()
                            })
                            .ToList();

                        var multasAgrupadas = multasPorUsuario
                            .GroupBy(m => new { m.NombreUsuario, m.FechaMulta.Month }) // Agrupamos por NombreUsuario y Mes
                            .ToList();

                        foreach (var group in multasAgrupadas)
                        {
                            var multa = new ActMulta
                            {
                                Id = group.First().Id,
                                IdUser = group.First().IdUser,
                                FechaMulta = group.First().FechaMulta,
                                NombreUsuario = group.Key.NombreUsuario
                            };

                            multa.NumeroMultas = group.Count();
                            multa.DetallesMulta = group.Select(a => new DetalleMulta
                            {
                                Valor = (decimal)a.Valor,
                                FechaMulta = a.FechaMulta,
                                Cuadrante = a.FechaMulta.Day <= 15 ? 1 : 2
                            }).ToList();

                            // Calculamos el valor total de las multas en el mes
                            multa.Valor = group.Sum(m => m.Valor);

                            multas.Add(multa);
                        }
                    }
                }
            }

            return multas;
        }
    }
}
