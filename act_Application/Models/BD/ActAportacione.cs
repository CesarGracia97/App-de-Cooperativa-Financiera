namespace act_Application.Models.BD
{
    public partial class ActAportacione
    {
        public int Id { get; set; }
        public string IdApor {  get; set; }
        public int IdUser { get; set; }
        public DateTime FechaAportacion { get; set; }
        public string Cuadrante { get; set; }
        public decimal Valor { get; set; }
        public string NBancoOrigen { get; set; }
        public string CBancoOrigen { get; set; }
        public string NBancoDestino { get; set; }
        public string CBancoDestino { get; set; }
        public byte[] CapturaPantalla { get; set; }
        public string Aprobacion { get; set; }

    }
}
