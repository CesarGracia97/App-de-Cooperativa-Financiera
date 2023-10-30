using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

public partial class ActTablaPorcentajePrestamo
{
    public int Id { get; set; }

    public int EscenarioId { get; set; }

    public int TipoPorcentajeId { get; set; }
}
