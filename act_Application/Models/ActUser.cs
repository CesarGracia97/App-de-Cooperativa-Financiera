using System.ComponentModel.DataAnnotations;

namespace act_Application.Models;

/// <summary>
/// Tabla de Usuarios
/// </summary>
public partial class ActUser
{
    public int Id { get; set; }

    public string Cedula { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "El correo electrónico contiene caracteres no permitidos.")]
    public string Correo { get; set; }

    public string NombreYapellido { get; set; }

    public string Celular { get; set; }

    public string Contrasena { get; set; }

    public string TipoUser { get; set; }

    public int IdSocio { get; set; }
}
