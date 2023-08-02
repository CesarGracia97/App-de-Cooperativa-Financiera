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

    public string Correo { get; set; } 

    public string NombreYapellido { get; set; } 

    public string Celular { get; set; }

    public string Contrasena { get; set; }

    public string TipoUser { get; set; } 

    public int Ncaccionario { get; set; }
}
