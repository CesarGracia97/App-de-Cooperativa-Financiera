namespace act_Application.Logic.ComplementosLogicos
{
    public class ObtenerTipoNotificacion
    {
        public string ClasificarInformacion(string cadena)
        {
            try
            {
                if (cadena.Length >= 4)
                {
                    // Verificar si los primeros 4 caracteres son alfabéticos seguidos por un guion
                    if (char.IsLetter(cadena[0]) && char.IsLetter(cadena[1]) && char.IsLetter(cadena[2]) && char.IsLetter(cadena[3]) && cadena[4] == '-')
                    {
                        // Obtener los primeros 4 caracteres después del guion
                        string tipo = cadena.Substring(0, 4);
                        // Información de Tipo Actividad
                        return ObtenerTipoActividad(tipo);
                    }

                    // Verificar si los primeros 4 caracteres son numéricos y la longitud es 10
                    if (char.IsDigit(cadena[0]) && char.IsDigit(cadena[1]) && char.IsDigit(cadena[2]) && char.IsDigit(cadena[3]) && cadena.Length == 10)
                    {
                        // Información de Tipo Registro de Cuenta
                        return "NUSE";
                    }
                }
                // Si no cumple ninguna condición, devolver un mensaje indicando que no se pudo clasificar
                return "No se pudo clasificar la información";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n--------------------------------------------------------------------");
                Console.WriteLine($"Error.");
                Console.WriteLine($"ObtenerTipoActividad | Razon del Error: {ex.Message}");
                Console.WriteLine($"--------------------------------------------------------------------\n");
                return "Error";
            }
        }

        private static string ObtenerTipoActividad(string cadena)
        {
            try
            {
                // Mapear los tipos y devolver el resultado correspondiente
                switch (cadena)
                {
                    case "APOR":
                        return "APOR"; //Aportes
                    case "MULT":
                        return "MULT"; //Multa Generada
                    case "AMUL":
                        return "AMUL"; //Multa Abonada
                    case "CMUL":
                        return "CMUL"; //Multa Cancelada
                    case "CUOT":
                        return "CUOT"; //Cuota Generada
                    case "ACUO":
                        return "ACUO"; //Cuota Abonada
                    case "CCUO":
                        return "CCUO"; //Cuota Cancelada
                    case "EVEN":
                        return "EVEN"; //Evento generado
                    case "PRES":
                        return "PRES"; //PrestamoGenerado
                    default:
                        return "Tipo de actividad no reconocido";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n--------------------------------------------------------------------");
                Console.WriteLine($"Error.");
                Console.WriteLine($"ObtenerTipoActividad | Razon del Error: {ex.Message}");
                Console.WriteLine($"--------------------------------------------------------------------\n");
                return "Error";
            }
        }
    }
}
