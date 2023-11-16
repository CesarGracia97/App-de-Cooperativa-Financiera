using act_Application.Models.BD;

namespace act_Application.Models.Sistema.ViewModel
{
    public class Transacciones_VM
    {
        public Transacciones_VM()
        {
            Aportaciones = new ActAportacione();
            Prestamos = new ActPrestamo();
            Cuotas = new ActCuota();
            Multas = new ActMulta();
        }
        public ActAportacione Aportaciones { get; set; }
        public ActPrestamo Prestamos { get; set; }
        public ActCuota Cuotas {  get; set; }
        public ActMulta Multas {  get; set; }
    }
}
