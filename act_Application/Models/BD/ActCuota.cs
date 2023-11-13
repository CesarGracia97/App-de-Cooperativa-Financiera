namespace act_Application.Models.BD
{
    public partial class ActCuota
    {
        public int Id { get; set; }
        public string IdCuot { get; set; }
        public int IdUser { get; set; }
        public int IdPrestamo { get; set; }
        public DateTime FechaCuota {  get; set; }
        public decimal Valor {  get; set; }
        public string Estado {  get; set; }
    }
}
