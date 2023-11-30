using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Models.Sistema.ViewModel
{
    public class Home_VM
    {
        public Home_VM()
        {

            AportacionesUser = new DetallesAportacionesUsers();
            MultaUser = new DetallesMultasUsers();
            PrestamosUser = new DetallesPrestamosUsers();
            Prestamos = new ActPrestamo();
            Eventos = new ActEvento();
            Cuotas = new ActCuota();

        }
        public DetallesPrestamosUsers PrestamosUser { get; set; }
        public DetallesAportacionesUsers AportacionesUser { get; set; }
        public DetallesMultasUsers MultaUser { get; set; }
        public ActEvento Eventos { get; set; }
        public ActPrestamo Prestamos { get; set; }
        public ActCuota Cuotas { get; set; }
    }
}
