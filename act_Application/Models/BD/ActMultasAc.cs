namespace act_Application.Models.BD
{
    /// <summary>
    /// Tabla de Liquidacion de Multas. MultasAC
    /// </summary>
    public partial class ActMultasAc
    {
        public int Id { get; set; }

        public int IdUser { get; set; }

        public string MultasIds { get; set; } = null!;

        public decimal ValorAc { get; set; }

        public decimal? LiquidacionTotal { get; set; }
    }

}
