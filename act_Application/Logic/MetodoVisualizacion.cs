namespace act_Application.Logic
{
    public class MetodoVisualizacion
    {
        public bool CondicionVisualizacion(int IdUser, string NameUser, string ColumnNPar, string ColumnIdPart)
        {
            // Dividir los valores en los vectores de participantes
            var nombresParticipantes = ColumnNPar.Split(", ");
            var idsParticipantes = ColumnIdPart.Split(", ");

            // Verificar si el Id del usuario y el Nombre del usuario coinciden con algún participante
            for (int i = 0; i < nombresParticipantes.Length; i++)
            {
                if (IdUser.ToString() == idsParticipantes[i] && NameUser == nombresParticipantes[i])
                {
                    return false; // El usuario es un participante
                }
            }

            return true; // El usuario no es un participante
        }
    }
}
