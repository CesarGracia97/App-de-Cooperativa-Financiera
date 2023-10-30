namespace act_Application.Models.BD
{
    /// <summary>
    /// Tabla de Liquidacion de Aportes. AportacionesAC
    /// </summary>
    public partial class ActAportacionesAc
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string AportacionesIds { get; set; }
        public decimal ValorAc { get; set; }
        public decimal? LiquidacionTotal { get; set; }
        public int NAportaciones { get; set; }
    }

}
