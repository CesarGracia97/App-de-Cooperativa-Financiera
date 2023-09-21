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
        public List<ListItems> ItemsNBanco { get; set; }
        public List<ListItems> ItemsRazon { get; set; }
        public List<ActCuentaDestino> ItemCuentaBancoDestino { get; set; }
    }
}

