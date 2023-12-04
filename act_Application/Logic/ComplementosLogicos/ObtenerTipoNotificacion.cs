namespace act_Application.Logic.ComplementosLogicos
{
    public class ObtenerTipoNotificacion
    {
        public string ClasificarInformacion(string cadena)
        {
            if (cadena.Length >= 4)
            {
                // Verificar si los primeros 4 caracteres son alfabéticos seguidos por un guion
                if (char.IsLetter(cadena[0]) && char.IsLetter(cadena[1]) && char.IsLetter(cadena[2]) && char.IsLetter(cadena[3]) && cadena[4] == '-')
                {
                    // Información de Tipo Actividad
                    return ObtenerTipoActividad(cadena);
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

        private static string ObtenerTipoActividad(string cadena)
        {
            // Obtener los primeros 4 caracteres después del guion
            string tipo = cadena.Substring(5, 4);

            // Mapear los tipos y devolver el resultado correspondiente
            switch (tipo)
            {
                case "APOR":
                    return "APOR";
                case "MULT":
                    return "MULT";
                case "CUOT":
                    return "CUOT";
                case "EVEN":
                    return "EVEN";
                case "PRES":
                    return "PRES";
                default:
                    return "Tipo de actividad no reconocido";
            }
        }
    }
}
