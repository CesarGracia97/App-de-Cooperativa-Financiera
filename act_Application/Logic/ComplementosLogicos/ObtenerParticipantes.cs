namespace act_Application.Logic.ComplementosLogicos
{
    public class ObtenerParticipantes
    {
        public bool NombresParticipantes(string ParticipantesId, string NombresPId)
        {
            var nombresParticipantes = NombresPId.Split(","); 
            var idsParticipantes = ParticipantesId.Split(",");
            if (nombresParticipantes.Length > 0 && idsParticipantes.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
