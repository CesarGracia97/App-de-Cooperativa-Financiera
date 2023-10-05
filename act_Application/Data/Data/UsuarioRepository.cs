using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Data
{
    public class UsuarioRepository
    {
        public ActUser GetDataUser(string Correo, string Contrasena)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string userQuery = ConfigReader.GetQuery("SelecUsuario");

            ActUser objeto = new ActUser();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = userQuery;
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Correo", Correo);
                cmd.Parameters.AddWithValue("@Contrasena", Contrasena);
                cmd.CommandType = CommandType.Text;

                connection.Open();

                using (MySqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        objeto = new ActUser()
                        {
                            Id = rd.GetInt32(rd.GetOrdinal("Id")),
                            Cedula = rd["Cedula"].ToString(),
                            Correo = rd["Correo"].ToString(),
                            NombreYapellido = rd["NombreYApellido"].ToString(),
                            TipoUser = rd["TipoUser"].ToString(),
                            IdRol = rd.GetInt32(rd.GetOrdinal("IdRol")),
                        };

                    }
                }
            }
            return objeto;
        }
        public ActRol GetDataRolUser(int idRol)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string roleQuery = ConfigReader.GetQuery("SelectRol"); ;

            ActRol objetoRol = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(roleQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            objetoRol = new ActRol()
                            {
                                Id = rd.GetInt32("Id"),
                                NombreRol = rd["NombreRol"].ToString(),
                                DescripcionRol = rd["DescripcionRol"].ToString()
                            };
                        }
                    }
                }
            }

            return objetoRol;
        }
    }
}
