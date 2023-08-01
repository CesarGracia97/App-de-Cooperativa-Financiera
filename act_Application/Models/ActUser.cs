using System;
using System.Collections.Generic;

namespace act_Application.Models;

/// <summary>
/// Tabla de Usuarios
/// </summary>
public partial class ActUser
{
    public int Id { get; set; }

    public int Cedula { get; set; }

    public int Cbancaria { get; set; }

    public string Correo { get; set; } = null!;

    public string NombreYapellido { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string TipoUser { get; set; } = null!;

    public int Ncaccionario { get; set; }
}
