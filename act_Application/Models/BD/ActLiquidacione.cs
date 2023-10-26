namespace act_Application.Models.BD
{
    public class ActLiquidacione
    {
        public int Id { get; set; }
        public int IdSocio { get; set; }
        public string AportacionesId { get; set; }
        public decimal InteresAportaciones { get; set; }
        public string PrestamosId { get; set; }
        public decimal InteresPrestamos { get; set; }
        public decimal InteresGlobalPrestamos { get; set; }
        public decimal InteresGlobalAportaciones { get; set; }
    }
}
