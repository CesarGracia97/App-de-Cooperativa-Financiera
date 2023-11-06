using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla de Usuarios
/// </summary>
public partial class ActUser
{
    public int Id { get; set; }

    public string Cedula { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string NombreYapellido { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string TipoUser { get; set; } = null!;

    public int IdSocio { get; set; }

    public byte[]? FotoPerfil { get; set; }

    public string Estado { get; set; } = null!;
}
