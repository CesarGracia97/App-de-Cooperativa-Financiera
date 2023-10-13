namespace act_Application.Models.Sistema.Complementos
{
    public partial class DetallesTransaccionesUsers
    {
        public class DetallesPorTransaccion
        {
            public int Id { get; set; }
            public string Razon { get; set; }
            public decimal Valor { get; set; }
            public string Estado { get; set; }
        }
        public List<DetallesPorTransaccion>  Detalles { get; set; }
        public int TotalTransacciones { get; set; }
        public int TotalCuotas { get; set; }
        public int TotalPagoUnico { get; set; }
        public int TotalAprobado { get; set; }
        public int TotalPendiente { get; set; }
        public int TotalRechazado { get; set; }
        public decimal ValorTotalPrestado {  get; set; }
        public DetallesTransaccionesUsers()
        {
            Detalles = new List<DetallesPorTransaccion>();
        }
    }
}
