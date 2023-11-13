namespace act_Application.Models.BD
{
    public partial class ActMulta
    {
        public int Id { get; set; }
        public string IdMult { get; set; }
        public int IdUser { get; set; }
        public DateTime FechaGeneracion {  get; set; }
        public string Cuadrante {  get; set; }
        public string Razon {  get; set; }
        public decimal Valor {  get; set; }
        public string Estado {  get; set; }
    }
}
