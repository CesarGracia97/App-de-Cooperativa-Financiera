namespace act_Application.Models.Sistema.Complementos
{
    public partial class DetallesMultasUsers
    {
        public class DetallesPorMulta
        {
            public int Id { get; set; }
            public decimal Valor { get; set; }
            public string Aprobacion { get; set; }
        }
        public List<DetallesPorMulta> Detalles { get; set; }
        public decimal MultasAcumuladas { get; set; }
        public int TotalMultas {  get; set; }
        public int TotalCancelados {  get; set; }
        public DetallesMultasUsers()
        {
            Detalles = new List<DetallesPorMulta>();
        }
    }
}
