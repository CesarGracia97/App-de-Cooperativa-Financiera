using act_Application.Data.Context;
using act_Application.Data.Data;
using act_Application.Logic.ComplementosLogicos;
using act_Application.Models.BD;
using act_Application.Services.ServiciosAplicativos;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace act_Application.Services.ServiciosAutomaticos
{
    public class SAGeneradorMultas : IHostedService
    {
        private readonly NotificacionesServices _nservices;
        private readonly InteresesServices _iservices;
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
                if (cuotas[i].Estado == "PENDIENTE")
                {
                    if (DateTime.Now > cuotas[i].FechaCuota)
                    {
                        if (cuotas[i].Valor > 0)
                        {
                            string Descripcion = $"Señor Usuario {cuotas[i].NombreDueño}, se le a Aplicado una multa a Razon del impago de la Cuota puesta para el dia {cuotas[i].FechaCuota}." +
                                $"\nPor favor pagar la Multa y la cuota lo mas pronto posible para evitar que aumente el valor de la sancion.";
                            string Razon = $"ID:{cuotas[i].IdCuot} - IMPAGO CUOTAS ";
                            await MandarMulta( 1, cuotas[i].Id, cuotas[i].IdPrestamo, cuotas[i].IdUser, cuotas[i].TipoUsuario, Razon, cuotas[i].Valor, new ActMulta());
                            await _nservices.CrearNotificacion(7, cuotas[i].IdUser, cuotas[i].IdCuot, "Aplicacion de Multa por Impago de Cuota", Descripcion, cuotas[i].IdUser.ToString(), new ActNotificacione());
                        }
                    }
                    else if( DateTime.Now > cuotas[i].FechaCuota.AddDays(61))
                    {
                        //Parte para interes superior del 61 dias pendiente para la sig semana.
                        string Descripcion = $"Señor Usuario {cuotas[i].NombreDueño}, el impago de la cuota a superado los 61 dias, se Aumento el valor de la multa " +
                                             $"a Razon del impago a largo plazo de la Cuota establecida para el dia {cuotas[i].FechaCuota}." +
                                             $"\nPor favor pagar la Multa y la cuota lo mas pronto posible para evitar que aumente el valor de la sancion.";
                        string Razon = $"ID:{cuotas[i].IdCuot} - IMPAGO CUOTAS ";
                        await MandarMulta(2, cuotas[i].Id, cuotas[i].IdPrestamo, cuotas[i].IdUser, cuotas[i].TipoUsuario, Razon, cuotas[i].Valor, new ActMulta());
                        await _nservices.CrearNotificacion(7, cuotas[i].IdUser, cuotas[i].IdCuot, "Aplicacion de Multa por Impago de Cuota", Descripcion, cuotas[i].IdUser.ToString(), new ActNotificacione());

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
        private async Task MandarMulta(int opcion, int Id, int IdPrestamo, int IdUser, string TipoUsuario, string Razon, decimal Valor, [Bind("Id,IdUser,FechaGeneracion,Cuadrante,Razon,Valor,Estado,FechaPago,CBancoOrigen,NBancoOrigen,CBancoDestino,NBancoDestino,HistorialValores,CapturaPantalla")] ActMulta actMulta)
        {
            decimal PorcentajeGarante = 0m;
            decimal PorcentajeTodos = 0m;

            actMulta.IdUser = IdUser;
            actMulta.FechaGeneracion = DateTime.Now;
            ObtenerCuadrante ocobj = new ObtenerCuadrante();
            actMulta.Cuadrante = ocobj.Cuadrante(DateTime.Now);
            actMulta.Razon = Razon;
            var erobj = new EventosRepository().A_GetParticipantesPrestamo(IdPrestamo);
            bool opobj = new ObtenerParticipantes().NombresParticipantes(erobj.ParticipantesId, erobj.NombresPId);
            decimal porcentaje = 0m;
            switch (opcion)
            {
                case 1:
                    if (TipoUsuario == "Socio" || TipoUsuario == "Administrador")
                    {
                        if (opobj)
                        {
                            //Socio o Admin Si participa alguien
                            porcentaje = 0.03m;
                            PorcentajeGarante = 0.0060m;
                            PorcentajeTodos = 0.0180m;
                        }
                        else
                        {
                            //Socio o AdminNo participa nadie
                            porcentaje = 0.03m;
                            PorcentajeGarante = 0m;
                            PorcentajeTodos = 0.03m;
                        }
                    }
                    else if (TipoUsuario == "Referido")
                    {
                        porcentaje = 0.10m;
                        /*
                        if (opobj)
                        {
                            //Referido Si participa alguien
                            porcentaje = 0.10m;
                        }
                        */
                        /*
                         else
                        {
                            //Referido No participa nadie
                            porcentaje = 0.10m;
                        }
                        */
                        PorcentajeGarante = 0.0240m;
                        PorcentajeTodos = 0.0360m;
                    }
                    break;
                case 2:
                    if (TipoUsuario == "Socio" || TipoUsuario == "Administrador")
                    {
                        if (opobj)
                        {
                            //Socio o Admin Si participa alguien
                            porcentaje = 0.06m;
                            PorcentajeGarante = 0.060m;
                            PorcentajeTodos = 0.0360m;
                        }
                        else
                        {
                            //Socio o AdminNo participa nadie
                            porcentaje = 0.06m;
                            PorcentajeGarante = 0m;
                            PorcentajeTodos = 0.06m;
                        }
                    }
                    else if (TipoUsuario == "Referido")
                    {
                        porcentaje = 0.14m;
                        /*
                        if (opobj)
                        {
                            //Referido Si participa alguien
                            porcentaje = 0.10m;
                        }
                        */
                        /*
                         else
                        {
                            //Referido No participa nadie
                            porcentaje = 0.10m;
                        }
                        */
                        PorcentajeGarante = 0.0240m;
                        PorcentajeTodos = 0.1160m;
                    }
                    break;
                default:
                    break;
            }

            actMulta.Valor = (Valor * porcentaje);
            actMulta.Estado = "ACTIVO";
            _context.Add(actMulta);
            await _context.SaveChangesAsync();
            await _iservices.AddNewInteres(actMulta.IdUser, "act_Multas", actMulta.Valor, PorcentajeGarante, PorcentajeTodos, new ActTablaInteres());
        }
    }
}
