using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla Relacion Rol Usuario
/// </summary>
public partial class ActRolUser
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public int IdRol { get; set; }
}
