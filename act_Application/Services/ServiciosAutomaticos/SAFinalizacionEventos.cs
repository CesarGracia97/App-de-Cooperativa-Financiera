using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace act_Application.Services
{
    public class SAFinalizacionEventos : IHostedService
    {
        private readonly ActDesarrolloContext _context;
        private Timer _timer;
        public SAFinalizacionEventos(ActDesarrolloContext context, Timer timer)
        {
            _context = context;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(FinalizarEventos, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
        public async void FinalizarEventos(object state)
        {
            var erobj = new EventosRepository();
            List<ActEvento> eventos = erobj.GetAllDataEventos();
            for (int i = 0; i < eventos.Count; i++)
            {
                if (eventos[i].Estado == "CONCURSO")
                {
                    if (DateTime.Now >= eventos[i].FechaFinalizacion)
                    {
                        await ActualizarEstadoEvento(eventos[i].Id, new ActEvento());
                    }
                }
            }
        }
        private async Task ActualizarEstadoEvento(int Id, [Bind("Id,IdEven,IdPrestamo,IdUser,ParticipantesId,NombresPId,FechaGeneracion,FechaInicio,FechaFinalizacion,Estado")] ActEvento actEvento)
        {
            if (Id != actEvento.Id)
            {
                Console.WriteLine("ActualizarEstadoEvento | Ocurrio un Error en la condicion de Comparacion Id = act.Id");
            }
            try
            {
                actEvento.Id = Id;
                actEvento.Estado = "FINALIZADO";
                _context.Update(actEvento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("ActualizarEstadoEvento | Error");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
        }
    }
}
