using act_Application.Helper;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Data
{
    public class UsuarioRepository
    {
        public ActUser GetDataUser(string Correo, string Contrasena)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string userQuery = ConfigReader.GetQuery("SelectUsuario");

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
                            Id = Convert.ToInt32(rd["Id"]),
                            Cedula = Convert.ToString(rd["Cedula"]),
                            Correo = Convert.ToString(rd["Correo"]),
                            NombreYapellido = Convert.ToString(rd["NombreYApellido"]),
                            TipoUser = Convert.ToString(rd["TipoUser"]),
                            IdRol = Convert.ToInt32(rd["IdRol"]),
                            Estado = Convert.ToString(rd["Estado"])
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
        public List<UserList> GetDataListUser()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            string Query = ConfigReader.GetQuery("SelectListUser");

            List<UserList> users = new List<UserList>();

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand(Query, conexion);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserList user = new UserList()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Usuario = Convert.ToString(reader["NombreYApellido"])
                        };
                        users.Add(user);
                    }
                }
            }
            return users;
        }
    }
}
