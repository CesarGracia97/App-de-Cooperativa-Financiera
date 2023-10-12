namespace act_Application.Models.Sistema.Complementos
{
    public partial class DetallesAportacionesUsers
    {
        public class DetallesPorAportacion
        {
            public int Id { get; set; }
            public decimal Valor { get; set; }
            public string Aprobacion { get; set; }
            public string Nbanco { get; set; }
        }
        public List<DetallesPorAportacion> Detalles { get; set; }
        public decimal AportacionesAcumuladas {  get; set; }
        public int TotalAportaciones {  get; set; }
        public int TotalAprobados { get; set; }
        public int TotalEspera { get; set; }
        public DetallesAportacionesUsers()
        {
            Detalles = new List<DetallesPorAportacion>();
        }
    }
}
