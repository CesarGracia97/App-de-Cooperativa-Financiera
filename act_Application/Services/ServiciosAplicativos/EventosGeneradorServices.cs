using act_Application.Data.Context;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace act_Application.Services.ServiciosAplicativos
{
    public class EventosGeneradorServices
    {
        private readonly ActDesarrolloContext _context;
        public EventosGeneradorServices(ActDesarrolloContext context)
        {
            _context = context;
        }
        public async Task CrearEvento(int Id, int IdUser, DateTime FechaInicio, DateTime FechaFinalizacion, [Bind("Id,IdPrestamo,Estado,FechaInicio,FechaFinalizacion,FechaGeneracion,ParticipantesId,ParticipantesNombre,IdUser")] ActEvento actParticipante)
        {
            try
            {
                actParticipante.IdPrestamo = Id;
                actParticipante.IdUser = IdUser;
                actParticipante.ParticipantesId = "";
                actParticipante.NombresPId = "";
                actParticipante.FechaGeneracion = DateTime.Now;
                actParticipante.FechaInicio = FechaInicio;
                actParticipante.FechaFinalizacion = FechaFinalizacion;
                actParticipante.Estado = "CONCURSO";
                /*ESTADOS:
                            CONCURSO (A LA ESPERA DE KUE NUEVOS SOCIOS SE UNAN).
                            FINALIZADO (EL CONCURSO TERMINO Y SE SELECCIONARON LOS PARTICIPANTES.*/
                _context.Add(actParticipante);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EventosGeneradorServices - CrearEvento | Problemas al momento de crear el Registro de la Evento");
                Console.WriteLine($"Detalles del error: {ex.Message}");
            }
        }

    }
}
