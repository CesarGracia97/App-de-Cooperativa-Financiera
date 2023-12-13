using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Models.Sistema.ViewModel
{
    public class Notificaciones_VM
    {
        public Notificaciones_VM()
        {
            Aportaciones = new ActAportacione();
            Prestamos = new ActPrestamo();
            Cuotas = new ActCuota();
            Multas = new ActMulta();
            Notificaciones = new ActNotificacione();
            Usuarios = new ActUser();
        }
        public ActNotificacione Notificaciones { get; set; }
        public ActPrestamo Prestamos { get; set; }
        public ActCuota Cuotas { get; set; }
        public ActAportacione Aportaciones { get; set; }
        public ActEvento Eventos { get; set; }
        public ActMulta Multas { get; set; }
        public ActUser Usuarios { get; set; }
        public List<ListItems> TipoUsuario { get; set; }
        public List<ListItems> Estado { get; set; }
    }
}
