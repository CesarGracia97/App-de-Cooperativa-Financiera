using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace act_Application.Models.BD;

/// <summary>
/// Tabla de Participantes/Garantes de Prestamo
/// </summary>
public partial class ActParticipante
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdTransaccion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinalizacion { get; set; }

    public DateTime FechaGeneracion { get; set; }

    public string ParticipantesId { get; set; }

    public string ParticipantesNombre { get; set; }

    public string Estado { get; set; }

    [NotMapped]
    public string NombreUsuario {  get; set; }
}
