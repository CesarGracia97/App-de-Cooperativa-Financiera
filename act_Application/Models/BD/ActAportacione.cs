using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace act_Application.Models.BD;

/// <summary>
/// Aportaciones ec
/// </summary>
public partial class ActAportacione
{
    public int Id { get; set; }

    public string Razon { get; set; } = null!;

    public decimal Valor { get; set; }

    public int IdUser { get; set; }

    public DateTime FechaAportacion { get; set; }

    public string Aprobacion { get; set; } = null!;

    [Column(TypeName = "longblob")]
    public byte[] CapturaPantalla { get; set; }

    public int Cuadrante1 { get; set; }

    public int Cuadrante2 { get; set; }

    public string Nbanco { get; set; } = null!;

    public string Cbancaria { get; set; } = null!;
    [NotMapped]
    public string NombreUsuario { get; set; }

    [NotMapped]
    public int NumeroAportaciones { get; set; }

    [NotMapped]
    public List<DetalleAportacion> DetallesAportaciones { get; set; }
}
public class DetalleAportacion
{
    public decimal Valor { get; set; }

    public DateTime FechaAportacion { get; set; }

    public int Cuadrante { get; set; }
}