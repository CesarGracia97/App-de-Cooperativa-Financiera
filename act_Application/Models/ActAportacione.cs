using System;
using System.Collections.Generic;

namespace act_Application.Models;

/// <summary>
/// Aportaciones ec
/// </summary>
public partial class ActAportacione
{
    public int Id { get; set; }

    public float Valor { get; set; }

    public int IdUser { get; set; }

    public DateTime FechaAportacion { get; set; }

    public string Aprobacion { get; set; } = null!;

    public byte[] CapturaPantalla { get; set; } = null!;
}
