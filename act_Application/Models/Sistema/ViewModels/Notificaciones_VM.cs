using act_Application.Models.BD;

namespace act_Application.Models.Sistema.ViewModels
{
    public class Notificaciones_VM
    {
        public Notificaciones_VM()
        {
            Notificaciones = new ActNotificacione();
            Transacciones = new ActPrestamo();
            Cuotas = new ActCuota();
            Aportaciones = new ActAportacione();
            Eventos = new ActEvento();
        }

        public ActNotificacione Notificaciones { get; set; }

        public ActPrestamo Transacciones { get; set; }

        public ActCuota Cuotas { get; set; }

        public ActAportacione Aportaciones { get; set; }

        public ActEvento Eventos { get; set; }
    }
}
