using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

public partial class ActPorcentaje
{
    public int Id { get; set; }

    public int TipoPorcentajeId { get; set; }

    public int EscenarioId { get; set; }

    public string Tipo { get; set; }

    public decimal Porcentaje { get; set; }

    public string Vporcentaje { get; set; }
}
