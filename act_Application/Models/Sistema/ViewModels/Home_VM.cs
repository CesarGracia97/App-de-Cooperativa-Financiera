using act_Application.Models.BD;

namespace act_Application.Models.Sistema.ViewModels
{
    public class Home_VM
    {
        public Home_VM() {

            Aportaciones = new ActAportacione();

            Eventos = new ActEvento();

            Multa = new ActMulta();

            Transacciones = new ActTransaccione();

            Cuotas = new ActCuota();

        }
        public ActAportacione Aportaciones { get; set; }

        public ActEvento Eventos { get; set; }

        public ActMulta Multa { get; set; }

        public ActTransaccione Transacciones { get; set; }

        public ActCuota Cuotas { get; set; }
    }
}
