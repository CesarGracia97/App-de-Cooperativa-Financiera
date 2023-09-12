using System;
using System.Collections.Generic;

namespace CodeGenerator.Models.BD;

/// <summary>
/// Tabla de Participantes/Garantes de Prestamo
/// </summary>
public partial class ActParticipante
{
    public int Id { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinalizacion { get; set; }

    public DateTime FechaGeneracion { get; set; }

    public string Participantes { get; set; } 

    public int IdTransacciones { get; set; }

    public string Estado { get; set; }
}