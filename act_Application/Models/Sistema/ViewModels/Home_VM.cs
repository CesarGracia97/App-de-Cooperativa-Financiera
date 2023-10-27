using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Models.Sistema.ViewModels
{
    public class Home_VM
    {
        public Home_VM() {

            AportacionesUser = new DetallesAportacionesUsers();
            MultaUser = new DetallesMultasUsers();
            PrestamosUser = new DetallesPrestamosUsers();
            Transacciones = new ActPrestamo();
            Eventos = new ActEvento();
            Cuotas = new ActCuota();

        }
        public DetallesPrestamosUsers PrestamosUser { get; set; }
        public DetallesAportacionesUsers AportacionesUser { get; set; }
        public DetallesMultasUsers MultaUser { get; set; }
        public ActEvento Eventos { get; set; }
        public ActPrestamo Transacciones { get; set; }
        public ActCuota Cuotas { get; set; }
    }
}
