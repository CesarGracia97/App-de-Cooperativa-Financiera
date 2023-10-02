using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Operaciones de Referentes
/// </summary>
public partial class ActTransaccione
{
    public int Id { get; set; }

    public string Razon { get; set; }

    public int IdUser { get; set; }

    public decimal Valor { get; set; }

    public string Estado { get; set; }

    public DateTime FechaEntregaDinero { get; set; }

    public DateTime FechaPagoTotalPrestamo { get; set; }

    public DateTime FechaIniCoutaPrestamo { get; set; }

    public DateTime FechaGeneracion { get; set; }

    public string TipoCuota { get; set; }

    public int IdParticipantes { get; set; }
}
