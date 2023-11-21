using act_Application.Data.Context;
using act_Application.Data.Data;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace act_Application.Services.ServiciosAutomaticos
{
    public class SAGeneradorMultas : IHostedService
    {
        private readonly NotificacionesServices _nservices;
        private readonly ActDesarrolloContext _context;
        private Timer _timer;
        public SAGeneradorMultas(ActDesarrolloContext context, Timer timer)
        {
            _context = context;
            _timer = timer;
        }
        public async void GeneradorMultas(object state)
        {
            var cobj = new CuotaRepository();
            List<ActCuota> cuotas = cobj.A_GetDateCuotasAll();
            for(int i =0; i < cuotas.Count; i++)
            {
                if (cuotas[i].Estado == "PENDITE")
                {
                    if(DateTime.Now > cuotas[i].FechaCuota)
                    {
                        if (cuotas[i].Valor > 0)
                        {
                            string Descripcion = "";
                            await MandarMulta( cuotas[i].Id, cuotas[i].IdUser, cuotas[i].IdCuot, new ActMulta());
                            await _nservices.CrearNotificacion(7, cuotas[i].IdUser, cuotas[i].IdCuot, "Aplicacion de Multa por Impago de Cuota", Descripcion, cuotas[i].IdUser.ToString(), new ActNotificacione());
                        }
                        else
                        {

                        }
                    }
                }
            }
        }


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
        private async Task MandarMulta(int Id, int IdUser, string Razon, [Bind()] ActMulta actMulta)
        {
            _context.Add(actMulta);
            await _context.SaveChangesAsync();
        }
    }
}
