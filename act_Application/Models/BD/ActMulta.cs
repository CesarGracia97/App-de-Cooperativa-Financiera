using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla de Multas
/// </summary>
public partial class ActMulta
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public decimal Porcentaje { get; set; }

    public decimal Valor { get; set; }

    public DateTime FechaMulta { get; set; }

    public int IdAportacion { get; set; }

    public int Cuadrante1 { get; set; }

    public int Cuadrante2 { get; set; }
   
    [NotMapped]
    public string NombreUsuario { get; set; }

    [NotMapped]
    public int NumeroMultas { get; set; }

    [NotMapped]
    public List<DetalleMulta> DetallesMulta { get; set; }


}

public class DetalleMulta
{
    public decimal Valor { get; set; }
    public DateTime FechaMulta { get; set; }
    public int Cuadrante { get; set; }
}