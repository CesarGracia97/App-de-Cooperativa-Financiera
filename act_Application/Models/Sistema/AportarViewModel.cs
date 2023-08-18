using act_Application.Models.BD;

namespace act_Application.Models.Sistema
{
    public class AportarViewModel
    {
        public AportarViewModel()
        {
            Aportacion = new ActAportacione(); // Inicializar aquí
        }
        public ActAportacione Aportacion { get; set; }
        public List<AportarItem> ItemsNBanco { get; set; }
        public List<AportarItem> ItemsRazon { get; set; }
    }
}

