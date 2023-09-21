using System;
using System.Collections.Generic;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla de Participantes/Garantes de Prestamo
/// </summary>
public partial class ActParticipante
{
    public int Id { get; set; }

    public int IdTransaccion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinalizacion { get; set; }

    public DateTime FechaGeneracion { get; set; }

    public string Participantes { get; set; } = null!;

    public string Estado { get; set; } = null!;
}
