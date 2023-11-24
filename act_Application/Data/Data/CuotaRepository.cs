﻿using act_Application.Helper;
using act_Application.Models.BD;
using MySql.Data.MySqlClient;
using System.Data;

namespace act_Application.Data.Data
{
    public class CuotaRepository
    {
        public ActCuota GetDataCuotasUser(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string Query = ConfigReader.GetQuery(1, "SelectCoutasUser");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = Query;
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdUser", IdUser);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            ActCuota obj = new ActCuota
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                IdCuot = Convert.ToString(rd["IdCuot"]),
                                IdUser = Convert.ToInt32(rd["IdUser"]),
                                IdPrestamo = Convert.ToInt32(rd["IdPrestamo"]),
                                FechaGeneracion = Convert.ToDateTime(rd["FechaGeneracion"]),
                                FechaCuota = Convert.ToDateTime(rd["FechaCuota"]),
                                Valor = Convert.ToDecimal(rd["Valor"]),
                                Estado = Convert.ToString(rd["Estado"]),
                                FechaPago = Convert.ToString(rd["CBancoOrigen"]),
                                CBancoOrigen = Convert.ToString(rd["CBancoOrigen"]),
                                NBancoOrigen = Convert.ToString(rd["NBancoOrigen"]),
                                CBancoDestino = Convert.ToString(rd["CBancoDestino"]),
                                NBancoDestino = Convert.ToString(rd["NBancoDestino"]),
                                HistorialValores = Convert.ToString(rd["HistorialValores"]),
                                CapturaPantalla = Convert.ToString(rd["CapturaPantalla"])
                            };
                            return obj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDataCuotasUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        public ActCuota GetIdDataCuotaUser(int Id)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            try
            {
                string Query = ConfigReader.GetQuery(1, "SelectIdCuota");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = Query;
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            ActCuota obj = new ActCuota
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                IdCuot = Convert.ToString(rd["IdCuot"]),
                                IdUser = Convert.ToInt32(rd["IdUser"]),
                                IdPrestamo = Convert.ToInt32(rd["IdPrestamo"]),
                                FechaGeneracion = Convert.ToDateTime(rd["FechaGeneracion"]),
                                FechaCuota = Convert.ToDateTime(rd["FechaCuota"]),
                                Valor = Convert.ToDecimal(rd["Valor"]),
                                Estado = Convert.ToString(rd["Estado"]),
                                FechaPago = Convert.ToString(rd["CBancoOrigen"]),
                                CBancoOrigen = Convert.ToString(rd["CBancoOrigen"]),
                                NBancoOrigen = Convert.ToString(rd["NBancoOrigen"]),
                                CBancoDestino = Convert.ToString(rd["CBancoDestino"]),
                                NBancoDestino = Convert.ToString(rd["NBancoDestino"]),
                                HistorialValores = Convert.ToString(rd["HistorialValores"]),
                                CapturaPantalla = Convert.ToString(rd["CapturaPantalla"])
                            };

                            return obj;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetIdDataCuotaUser | Error.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return null;
        }
        public int H_GetLastIdCouta(int IdUser)
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            int Id = -1;
            try
            {
                string Query = ConfigReader.GetQuery(2, "SelectLastIdCoutaUser");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = Query;
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdUse", IdUser);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            Id = Convert.ToInt32(rd["Id"]);
                        };
                    }
                }
                return Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("H_GetLastIdCouta | Error. ");
                Console.WriteLine("Detalles del error: " + ex.Message);
                return Id;
            }

        }
        public List<ActCuota> A_GetDateCuotasAll()
        {
            string connectionString = AppSettingsHelper.GetConnectionString();
            List<ActCuota> cuotasList = new List<ActCuota>();
            try
            {
                string Query = ConfigReader.GetQuery(3, "SelectDateCuotasAll");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(Query, connection);
                    cmd.CommandType = CommandType.Text;

                    connection.Open();

                    using (MySqlDataReader rd = cmd.ExecuteReader())
                    {
                        ActCuota obj = new ActCuota
                        {
                            Id = Convert.ToInt32(rd["Id"]),
                            IdPrestamo = Convert.ToInt32(rd["IdPrestamo"]),
                            IdUser = Convert.ToInt32(rd["IdUser"]),
                            IdCuot = Convert.ToString(rd["IdCuot"]),
                            FechaCuota = Convert.ToDateTime(rd["FechaCuota"]),
                            Valor = Convert.ToDecimal(rd["Valor"]),
                            NombreDueño = Convert.ToString(rd["NombreYApellido"]),
                            TipoUsuario = Convert.ToString(rd["TipoUser"])
                        };
                        cuotasList.Add(obj);
                    }
                    return cuotasList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A_GetDateCuotasAll | Error \n");
                Console.WriteLine($"Detalles del error: " + ex.Message);
                return null;
            }

        }
    }
}
