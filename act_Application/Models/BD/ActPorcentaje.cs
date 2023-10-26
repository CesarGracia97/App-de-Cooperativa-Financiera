namespace act_Application.Models.BD
{
    public partial class ActPorcentaje
    {
        public int Id { get; set; }
        public string Categoria { get; set; }
        public string Target { get; set; }
        public decimal Porcentaje { get; set; }
        public string Razon { get; set; }
        public string Condicion { get; set; }
    }
}
