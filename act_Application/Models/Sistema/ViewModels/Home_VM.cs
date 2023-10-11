using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Models.Sistema.ViewModels
{
    public class Home_VM
    {
        public Home_VM() {

            AportacionesUser = new DetallesAportacionesUsers();

            MultaUser = new DetallesMultasUsers();

            Eventos = new ActEvento();

            Transacciones = new ActTransaccione();

            Cuotas = new ActCuota();

        }
        public DetallesAportacionesUsers AportacionesUser { get; set; }
        public DetallesMultasUsers MultaUser { get; set; }
        public ActEvento Eventos { get; set; }
        public ActTransaccione Transacciones { get; set; }
        public ActCuota Cuotas { get; set; }
    }
}
