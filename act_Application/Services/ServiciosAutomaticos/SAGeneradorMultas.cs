using act_Application.Data.Data;
using act_Application.Helper;

namespace act_Application.Services.ServiciosAutomaticos
{
    public class SAGeneradorMultas : IHostedService
    {
        public void GeneradorMultas(object state)
        {
            var cobj = new CuotaRepository().A_GetDateCuotasAll();
        }
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(GeneradorMultas, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
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
    }
}
