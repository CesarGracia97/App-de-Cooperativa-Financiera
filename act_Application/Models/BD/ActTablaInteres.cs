namespace act_Application.Models.BD;
public partial class ActTablaInteres
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public string IdPersonalizado { get; set; }
    public string Porcentaje { get; set; }
    public decimal Valor { get; set; }
    public string Estado { get; set; }
}