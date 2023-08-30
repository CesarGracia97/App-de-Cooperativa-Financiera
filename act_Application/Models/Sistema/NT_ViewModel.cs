using act_Application.Models.BD;

namespace act_Application.Models.Sistema
{
    public class NT_ViewModel
    {
        public NT_ViewModel() {
            Notificaciones = new ActNotificacione();
            Transacciones = new ActTransaccione();
        }

        public ActNotificacione Notificaciones { get; set; }

        public ActTransaccione Transacciones { get; set; }
    }
}
