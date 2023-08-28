using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla de Notificaciones
/// </summary>
public partial class ActNotificacione
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public string Razon { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaNotificacion { get; set; }

    public string Destino { get; set; }

}
