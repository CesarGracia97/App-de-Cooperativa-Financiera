using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla Relacion:Liquidacion,Aportes,Prestamos,Multas,Usuario
/// </summary>
public partial class ActLapmt
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdAportacionesAc { get; set; }

    public int IdMultasAc { get; set; }

    public int IdPrestamosAc { get; set; }
}
