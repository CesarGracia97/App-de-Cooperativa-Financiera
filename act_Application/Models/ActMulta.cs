using System;
using System.Collections.Generic;

namespace act_Application.Models;

public partial class ActMulta
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public float Porcentaje { get; set; }

    public int IdAportacion { get; set; }
}
