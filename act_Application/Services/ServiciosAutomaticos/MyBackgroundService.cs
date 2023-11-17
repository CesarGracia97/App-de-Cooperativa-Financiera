using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace act_Application.Services.ServiciosAutomaticos
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("El servicio en segundo plano ha comenzado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Coloca aquí el código que deseas que se ejecute cada 5 minutos
                _logger.LogInformation("Este código se ejecuta cada 1 minuto - {0}", DateTime.Now);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("El servicio en segundo plano se ha detenido.");
        }
    }
}
