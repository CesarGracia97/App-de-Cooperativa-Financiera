using System;
using System.Collections.Generic;

namespace act_Application.Models;

public partial class ActMulta
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public decimal Porcentaje { get; set; }

    public decimal Valor { get; set; }

    public DateTime  FechaMulta { get; set; } 

    public int IdAportacion { get; set; }

    public int Cuadrante1 { get; set; }

    public int Cuadrante2 { get; set; }

}
