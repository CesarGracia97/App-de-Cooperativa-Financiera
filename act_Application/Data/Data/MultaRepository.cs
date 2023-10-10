using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Data
{
    public class MultaRepository
    {
        public bool GetExistMultas()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string MultaQuery = ConfigReader.GetQuery("SelectMultas");

            int totalMultas = 0; // Variable para almacenar el valor de TotalAportaciones

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(MultaQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read()) // Avanzar al primer registro
                        {
                            totalMultas = Convert.ToInt32(rd["TotalMultas"]);
                        }
                    }
                }
            }
            // Si totalAportaciones es mayor que 0, devuelve true, de lo contrario, devuelve false.
            return totalMultas > 0;
        }

        public List<ActMulta> GetDataMultas()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string multasQuery = ConfigReader.GetQuery("SelectMultas");

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
    
        public bool GetExistMultasUser(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string aportacionesQuery = ConfigReader.GetQuery("SelectAportacionesUser");

            int totalMultas = 0;

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
                            totalMultas = Convert.ToInt32(reader["TotalMultas"]);
                        }
                    }
                }
            }
            return totalMultas > 0;
        }

        public ActMulta GetDataMultasUser(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string aportacionesQuery = ConfigReader.GetQuery("SelectAportacionesUser");

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
                            ActMulta aportacione = new ActMulta
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Valor = Convert.ToDecimal(reader["Valor"]),
                                Aprobacion = reader["Aprobacion"].ToString()
                            };
                            return aportacione;
                        }
                    }
                }
            }
            return null;
        }
    }
}
