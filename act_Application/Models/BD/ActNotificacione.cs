namespace act_Application.Models.BD;
public partial class ActNotificacione
{
    public int Id { set; get; }
    public int IdUser { set; get; }
    public string IdActividad { set; get; }
    public DateTime FechaGeneracion { set; get; }
    public string Razon { set; get; }
    public string Descripcion { set; get; }
    public string Destino { set; get; }
    public string Visto { set; get; }
}
