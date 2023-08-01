using System;
using System.Collections.Generic;

namespace act_Application.Models;

/// <summary>
/// Movimientos
/// </summary>
public partial class ActTransaccione
{
    public int Id { get; set; }

    public string Razon { get; set; } = null!;

    public int IdUser { get; set; }
}
