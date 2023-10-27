namespace act_Application.Models.Sistema.Complementos
{
    public partial class DetallesPrestamosUsers
    {
        public class DetallesPorPrestamo
        {
            public int Id { get; set; }
            public string Razon { get; set; }
            public decimal Valor { get; set; }
            public string Estado { get; set; }
        }
        public List<DetallesPorPrestamo>  Detalles { get; set; }
        public int TotalPrestamos { get; set; }
        public int TotalCuotas { get; set; }
        public int TotalPagoUnico { get; set; }
        public int TotalAprobado { get; set; }
        public int TotalPendiente { get; set; }
        public int TotalRechazado { get; set; }
        public decimal ValorTotalPrestado {  get; set; }
        public DetallesPrestamosUsers()
        {
            Detalles = new List<DetallesPorPrestamo>();
        }
    }
}
