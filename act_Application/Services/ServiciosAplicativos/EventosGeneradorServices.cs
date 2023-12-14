using act_Application.Data.Context;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;

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

            }
            catch (Exception ex)
            {
                Console.WriteLine($"EventosGeneradorServices - CrearEvento | Problemas al momento de crear el Registro de la Evento");
                Console.WriteLine($"Detalles del error: {ex.Message}");
            }
        }

    }
}
