namespace act_Application.Models.BD;

/// <summary>
/// Tabla de Notificaciones
/// </summary>
public partial class ActNotificacione
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public string Razon { get; set; } 

    public string Descripcion { get; set; }

    public DateTime FechaNotificacion { get; set; }

    public string Destino { get; set; }

    public int IdTransacciones { get; set; }

    public int IdAportaciones { get; set; }

    public int IdCuotas { get; set; }
}