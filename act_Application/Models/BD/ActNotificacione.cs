using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla de Index
/// </summary>
public partial class ActNotificacione
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public string Razon { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaNotificacion { get; set; }

    public string Destino { get; set; } = null!;

    public int IdTransacciones { get; set; }

    public int IdAportaciones { get; set; }

    public int IdCuotas { get; set; }

    public int Visto { get; set; }
}
