using act_Application.Helper;
using act_Application.Logic.ComplementosLogicos;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Repository
{
    public class NotificacionesRepository
    {
        private readonly string connectionString = AppSettingsHelper.GetConnectionString();
        private bool GetExist_NotificacionesAdmin()
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "NOTI", "DBQN_SelectAdmiNotificacion");
                int TotalN = 0;
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
                                TotalN = Convert.ToInt32(reader["TotalNotificaciones"]);
                            }
                        }
                    }
                }
                return TotalN > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExistNotificacionesAdmin || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetExistNotificacionesAdmin || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return false;
            }

        }
        private bool GetExist_NotificacionesUser(int IdUser)
        {
            try
            {
                string Query = ConfigReader.GetQuery(1, "NOTI", "DBQN_SelectUserNotificacion");
                int TotalN = 0;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(Query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Id", IdUser);
                        connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Avanzar al primer registro
                            {
                                TotalN = Convert.ToInt32(reader["TotalNotificaciones"]);
                            }
                        }
                    }
                }
                return TotalN > 0;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetExistNotificacionesUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetExistNotificacionesUser || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return false;
            }
        }
        private List<ActNotificacione> GetData_NotificacionesAdmin() //Consulta para obtener todos los datos de las notificacionesAdmin del administrador
        {
            try
            {
                List<ActNotificacione> notifiAdmin = new List<ActNotificacione>();
                string Query = ConfigReader.GetQuery(1, "NOTI", "DBQN_SelectAdmiNotificacion");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(Query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ActNotificacione obj = MapToNotificaciones(reader);
                                notifiAdmin.Add(obj);
                                UsuarioRepository uobj = new UsuarioRepository();
                                AportacionRepository aobj = new AportacionRepository();
                                PrestamosRepository pobj = new PrestamosRepository();
                                MultaRepository mobj = new MultaRepository();
                                CuotaRepository cobj = new CuotaRepository();
                                EventosRepository eobj = new EventosRepository();
                                switch (new ObtenerTipoNotificacion().ClasificarInformacion(obj.IdActividad))
                                {
                                    case "NUSE":
                                        uobj.OperacionesUsuario(6, (int) new UsuarioRepository().OperacionesUsuario( 5, 0, 0, obj.IdActividad, ""), 0, "", "");
                                        break;
                                    case "APOR":
                                        aobj.OperacionesAportaciones(7, (int) new AportacionRepository().OperacionesAportaciones( 6, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "PRES":
                                        pobj.OperacionesPrestamos(2, (int) new PrestamosRepository().OperacionesPrestamos(1, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "MULT":
                                        mobj.OperacionesMultas(5, (int) new MultaRepository().OperacionesMultas( 7, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "CMUL":
                                        mobj.OperacionesMultas(5, (int) new MultaRepository().OperacionesMultas(7, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "AMUL":
                                        mobj.OperacionesMultas(5, (int) new MultaRepository().OperacionesMultas(7, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "CUOT":
                                        cobj.OperacionesCuotas(2, (int) new CuotaRepository().OperacionesCuotas(6, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "CCUO":
                                        cobj.OperacionesCuotas(2, (int) new CuotaRepository().OperacionesCuotas(6, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "ACUO":
                                        cobj.OperacionesCuotas(2, (int) new CuotaRepository().OperacionesCuotas(6, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "EVEN":
                                        eobj.OperacionesEventos(3, (int) new EventosRepository().OperacionesEventos(5, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    default:
                                        Console.WriteLine($"\n----------------------------------------------------------------------------------------------");
                                        Console.WriteLine($"GetData_NotificacionesAdmin | Error, Opcion Inexistente. Tipo de IdActividad: {obj.IdActividad}");
                                        Console.WriteLine($"\n----------------------------------------------------------------------------------------------");
                                        break;
                                }
                            }
                        }
                    }
                }
                return notifiAdmin;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetDataNotificacionesAdmin || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetDataNotificacionesAdmin || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return null;
            }
        }
        private List<ActNotificacione> GetData_NotificacionesUser(int IdUser) //Consulta para obtener todos los datos de las notificacionesUser del administrador
        {
            try
            {
                List<ActNotificacione> notifiUser = new List<ActNotificacione>();
                string Query = ConfigReader.GetQuery( 1, "NOTI", "DBQN_SelectUserNotificacion");
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
                                ActNotificacione obj = MapToNotificaciones(reader);
                                notifiUser.Add(obj);
                                UsuarioRepository uobj = new UsuarioRepository();
                                AportacionRepository aobj = new AportacionRepository();
                                PrestamosRepository pobj = new PrestamosRepository();
                                MultaRepository mobj = new MultaRepository();
                                CuotaRepository cobj = new CuotaRepository();
                                EventosRepository eobj = new EventosRepository();
                                switch (new ObtenerTipoNotificacion().ClasificarInformacion(obj.IdActividad))
                                {
                                    case "NUSE":
                                        uobj.OperacionesUsuario(6, (int)new UsuarioRepository().OperacionesUsuario(5, 0, 0, obj.IdActividad, ""), 0, "", "");
                                        break;
                                    case "APOR":
                                        aobj.OperacionesAportaciones(7, (int)new AportacionRepository().OperacionesAportaciones(6, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "PRES":
                                        pobj.OperacionesPrestamos(2, (int)new PrestamosRepository().OperacionesPrestamos(1, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "MULT":
                                        mobj.OperacionesMultas(5, (int)new MultaRepository().OperacionesMultas(7, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "CMUL":
                                        mobj.OperacionesMultas(5, (int)new MultaRepository().OperacionesMultas(7, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "AMUL":
                                        mobj.OperacionesMultas(5, (int)new MultaRepository().OperacionesMultas(7, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "CUOT":
                                        cobj.OperacionesCuotas(2, (int)new CuotaRepository().OperacionesCuotas(6, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "CCUO":
                                        cobj.OperacionesCuotas(2, (int)new CuotaRepository().OperacionesCuotas(6, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "ACUO":
                                        cobj.OperacionesCuotas(2, (int)new CuotaRepository().OperacionesCuotas(6, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    case "EVEN":
                                        eobj.OperacionesEventos(3, (int)new EventosRepository().OperacionesEventos(5, 0, 0, obj.IdActividad), 0, "");
                                        break;
                                    default:
                                        Console.WriteLine($"\n----------------------------------------------------------------------------------------------");
                                        Console.WriteLine($"GetData_NotificacionesAdmin | Error, Opcion Inexistente. Tipo de IdActividad: {obj.IdActividad}");
                                        Console.WriteLine($"\n----------------------------------------------------------------------------------------------");
                                        break;
                                }
                            }
                        }
                    }
                }
                return notifiUser;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetDataNotificacionesUser || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetDataNotificacionesUser || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return null;
            }
        }
        private ActNotificacione GetData_NotificacionesId(int Id)
        {
            try
            {
                ActNotificacione nobj = new ActNotificacione();
                string Query = ConfigReader.GetQuery(1, "NOTI", "DBQN_SelectNotificacionesId");
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(Query, connection);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            ActNotificacione obj = MapToNotificaciones(reader);
                            nobj = obj;
                        }
                    }
                }
                return nobj;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\nGetData_NotificacionesId || Error de Mysql");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nGetData_NotificacionesId || ErrorGeneral");
                Console.WriteLine($"\nRazon del Error: {ex.Message}\n");
                return null;
            }
        }
        private ActNotificacione MapToNotificaciones(MySqlDataReader reader)
        {
            return new ActNotificacione
            {
                Id = Convert.ToInt32(reader["Id"]),
                IdUser = Convert.ToInt32(reader["IdUser"]),
                IdActividad = Convert.ToString(reader["IdActividad"]),
                FechaGeneracion = Convert.ToDateTime(reader["FechaGeneracion"]),
                Razon = Convert.ToString(reader["Razon"]),
                Descripcion = Convert.ToString(reader["Descripcion"]),
                Destino = Convert.ToString(reader["Destino"]),
                Visto = Convert.ToString(reader["Visto"])
            };
        }
        public object OperacionesNotificaciones(int Opciones, int Id, int IdUser)
        {
            try
            {
                switch (Opciones)
                {
                    case 1:
                        return GetExist_NotificacionesAdmin();
                    case 2:
                        return GetExist_NotificacionesUser(IdUser);
                    case 3:
                        return GetData_NotificacionesAdmin();
                    case 4:
                        return GetData_NotificacionesUser(IdUser);
                    case 5:
                        return GetData_NotificacionesId(Id);
                    default:
                        Console.WriteLine("\n---------------------------------------------------");
                        Console.WriteLine("\nOperacionesNotificaciones || Opcion Inexistente.");
                        Console.WriteLine("\n---------------------------------------------------\n");
                        return null;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("\nOperacionesNotificaciones || Error.");
                Console.WriteLine("\nRazon del Error: " + ex.Message);
                Console.WriteLine("\n-----------------------------------");
                return null;
            }
        }
    }
}
