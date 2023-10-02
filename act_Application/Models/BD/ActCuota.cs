using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla de Cuotas, aqui se almacena el Id del Usuario, el Id d
/// </summary>
public partial class ActCuota
{
    public int Id { get; set; }

    public decimal ValorCuota { get; set; }

    public DateTime FechaCuota { get; set; }

    public string Estado { get; set; }

    public int IdUser { get; set; }

    public int IdTransaccion { get; set; }
}
