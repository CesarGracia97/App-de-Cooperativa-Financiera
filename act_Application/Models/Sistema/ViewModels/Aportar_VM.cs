using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Models.Sistema.ViewModels
{
    public class Aportar_VM
    {
        public Aportar_VM()
        {
            Aportacion = new ActAportacione(); // Inicializar aquí
        }
        public ActAportacione Aportacion { get; set; }
        public List<ListItems> ItemsNBanco { get; set; }
        public List<ListItems> ItemsRazon { get; set; }
        public List<ActCuentaDestino> ItemCuentaBancoDestino { get; set; }
    }
}

