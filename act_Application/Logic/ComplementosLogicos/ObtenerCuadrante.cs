namespace act_Application.Logic.ComplementosLogicos
{
    public class ObtenerCuadrante
    {
        public string Cuadrante(DateTime fecha)
        {
            int diasEnElMes = DateTime.DaysInMonth(fecha.Year, fecha.Month);

            if (fecha.Day >= 1 && fecha.Day <= 15)
            {
                return "Cuadrante 1";
            }
            else if (fecha.Day > 15 && fecha.Day <= diasEnElMes)
            {
                return "Cuadrante 2";
            }
            else
            {
                throw new ArgumentException("La fecha proporcionada no es válida.");
            }
        }
    }
}
