using act_Application.Models.BD;

namespace act_Application.Logic.ComplementosLogicos
{
    public static class ObtenerCuadrante
    {
        public static void CalcularCuadrantesAportacione(ActAportacione aportacione)
        {
            aportacione.Cuadrante1 = (aportacione.FechaAportacion.Day <= 15) ? 1 : 0;
            aportacione.Cuadrante2 = (aportacione.FechaAportacion.Day > 15) ? 1 : 0;
        }

        public static void CalcularCuadrantesMulta(ActMulta multa)
        {
            multa.Cuadrante1 = (multa.FechaMulta.Day <= 15) ? 1 : 0;
            multa.Cuadrante2 = (multa.FechaMulta.Day > 15) ? 1 : 0;
        }
    }
}
