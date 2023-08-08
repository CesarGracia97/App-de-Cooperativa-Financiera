using act_Application.Helper;
using act_Application.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Logica
{
    public class MetodoAportaciones
    {
        public List<ActAportacione> ObtenerAportaciones()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string aportacionesQuery = ConfigReader.GetQuery("SelectAportacion");

            List<ActAportacione> aportaciones = new List<ActAportacione>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(aportacionesQuery, connection))
                {
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            ActAportacione aportacion = new ActAportacione()
                            {
                                Id = rd.GetInt32("Id"),
                                Razon = rd["Razon"].ToString(),
                                Valor = rd.GetFloat("Valor"),
                                IdUser = rd.GetInt32("IdUser"),
                                FechaAportacion = rd.GetDateTime("FechaAportacion"),
                                Aprobacion = rd["Aprobacion"].ToString(),
                                CapturaPantalla = rd.IsDBNull(rd.GetOrdinal("CapturaPantalla")) ? null : (byte[])rd["CapturaPantalla"]
                            };
                            aportaciones.Add(aportacion);
                        }
                    }
                }
            }

            return aportaciones;
        }
    }
}