namespace act_Application.Models.BD
{
    public class ActHistorialLiquidacione
    {
        public int Id { get; set; }
        public int IdSocio { get; set; }
        public DateTime FechaRegistro {  get; set; }
        public string AportacionesId { get; set; }
        public decimal InteresAportaciones { get; set; }
        public string PrestamosId { get; set; }
        public decimal InteresesPrestamos { get; set; }
        public decimal InteresGlobalPrestamos { get; set; }
        public decimal InteresGlobalAportaciones { get; set; }
    }
}
