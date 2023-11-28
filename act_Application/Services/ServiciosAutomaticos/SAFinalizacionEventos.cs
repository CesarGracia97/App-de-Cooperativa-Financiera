using act_Application.Data.Context;
using act_Application.Data.Data;
using act_Application.Models.BD;
using act_Application.Services.ServiciosAutomaticos;

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
            // Calcular el tiempo hasta la próxima ejecución a la hora deseada (por ejemplo, a las 2:00 AM).
            var now = DateTime.Now;
            var desiredTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0); // 0:00 AM
            var initialDelay = desiredTime > now ? desiredTime - now : TimeSpan.FromHours(24) + (desiredTime - now);

            // Configurar el temporizador para que se ejecute una vez al día a la hora deseada.
            _timer = new Timer(FinalizarEventos, null, initialDelay, TimeSpan.FromHours(24));

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
            for(int i = 0; i < eventos.Count; i++)
            {
                if (eventos[i].Estado == "CONCURSO")
                {
                    if(DateTime.Now >= eventos[i].FechaFinalizacion)
                    {

                    }
                }
            }
        }
    }
}
