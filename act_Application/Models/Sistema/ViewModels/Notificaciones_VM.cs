using act_Application.Models.BD;

namespace act_Application.Models.Sistema.ViewModels
{
    public class Notificaciones_VM
    {
        public Notificaciones_VM()
        {
            Notificaciones = new ActNotificacione();
            Transacciones = new ActTransaccione();
            Cuotas = new ActCuota();
            Aportaciones = new ActAportacione();
            Eventos = new ActEvento();
        }

        public ActNotificacione Notificaciones { get; set; }

        public ActTransaccione Transacciones { get; set; }

        public ActCuota Cuotas { get; set; }

        public ActAportacione Aportaciones { get; set; }

        public ActEvento Eventos { get; set; }
    }
}
