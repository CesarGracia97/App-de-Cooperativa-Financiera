using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

public partial class ActTipoPorcentaje
{
    public int Id { get; set; }

    public int PorcentajeId { get; set; }

    public string NombreTipoPorcentaje { get; set; }
}
