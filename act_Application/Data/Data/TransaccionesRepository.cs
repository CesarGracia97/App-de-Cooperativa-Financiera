using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;

namespace act_Application.Data.Data
{
    public class TransaccionesRepository
    {

        public ActTransaccione GetTransaccionPorId(int idTransacciones) //Consulta para obtener todos los datos de una transaccion especifica
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string query = ConfigReader.GetQuery("SelecInfoTransacciones");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdTransacciones", idTransacciones);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ActTransaccione transaccion = new ActTransaccione
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Razon = reader["Razon"].ToString(),
                                    IdUser = Convert.ToInt32(reader["IdUser"]),
                                    Valor = Convert.ToDecimal(reader["Valor"]),
                                    Estado = reader["Estado"].ToString(),
                                    FechaEntregaDinero = Convert.ToDateTime(reader["FechaEntregaDinero"]),
                                    FechaPagoTotalPrestamo = Convert.ToDateTime(reader["FechaPagoTotalPrestamo"]),
                                    FechaIniCoutaPrestamo = Convert.ToDateTime(reader["FechaIniCoutaPrestamo"]),
                                    TipoCuota = reader["TipoCuota"].ToString(),
                                    IdParticipantes = Convert.ToInt32(reader["IdParticipantes"]),
                                    FechaGeneracion = Convert.ToDateTime(reader["FechaEntregaDinero"])
                                };
                                return transaccion;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hubo un error en la consulta de la transacción");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }

            return null;
        }


    }
}
