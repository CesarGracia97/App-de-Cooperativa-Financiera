using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace act_Application.Services.ServiciosAplicativos
{
    public class CapturaDePantallaServices
    {
        private readonly ActDesarrolloContext _context;
        public CapturaDePantallaServices(ActDesarrolloContext context)
        {
            _context = context;
        }
        public async Task SubirCapturaDePantalla(int IdUser, string Origen, int IdOrigenCaptura, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdUser,Origen,IdOrigen,CapturaPantalla")] ActCapturasPantalla actCapturasPantalla)
        {
            actCapturasPantalla.IdUser = IdUser;
            actCapturasPantalla.Origen = Origen;
            actCapturasPantalla.IdOrigenCaptura = IdOrigenCaptura;
            if (CapturaPantalla != null && CapturaPantalla.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await CapturaPantalla.CopyToAsync(ms);
                    var bytes = ms.ToArray();
                    actCapturasPantalla.CapturaPantalla = bytes; // Asigna los bytes de la imagen a la propiedad CapturaPantalla
                }
            }
            _context.Add(actCapturasPantalla);
            await _context.SaveChangesAsync();
        }
        public async Task ActualizarIdCapturaPantallaUser(int opcion, int Id, int IdCapturaPantalla, [Bind("Id,IdCuot,IdUser,IdPrestamo,FechaCuota,Valor,Estado,FechaPago,CBancoOrigen,NBancoOrigen,CBancoDestino,NBancoDestino,HistorialValores,CapturaPantalla")] ActCuota actCuota, [Bind("Id,IdMult,IdUser,FechaGeneracion,Cuadrante,Razon,Valor,Estado,FechaPago,CBancoOrigen,NBancoOrigen,CBancoDestino,NBancoDestino,HistorialValores,CapturaPantalla")] ActMulta actMulta)
        {
            switch (opcion)
            {
                case 1:
                    //CUOTAS
                    try
                    {
                        var cuotOriginal = (ActCuota) new CuotaRepository().OperacionesCuotas(2, Id, 0);
                        if (cuotOriginal != null)
                        {
                            actCuota.Id = cuotOriginal.Id;
                            actCuota.IdCuot = cuotOriginal.IdCuot;
                            actCuota.IdPrestamo = cuotOriginal.IdPrestamo;
                            actCuota.FechaCuota = cuotOriginal.FechaCuota;
                            actCuota.Valor = cuotOriginal.Valor;
                            actCuota.Estado = cuotOriginal.Estado;
                            actCuota.FechaPago = cuotOriginal.FechaPago;
                            actCuota.CBancoOrigen = cuotOriginal.CBancoOrigen;
                            actCuota.NBancoOrigen = cuotOriginal.NBancoOrigen;
                            actCuota.CBancoDestino = cuotOriginal.CBancoDestino;
                            actCuota.NBancoDestino = cuotOriginal.NBancoDestino;
                            actCuota.HistorialValores = cuotOriginal.HistorialValores;
                            actCuota.CapturaPantalla = cuotOriginal.CapturaPantalla + IdCapturaPantalla + ",";
                            _context.Update(actCuota);
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine("Hubo un problema al actualizar el campo de Captura de Pantalla de Cuota.");
                        Console.WriteLine("Detalles del error: " + ex.Message);
                    }

                    break;
                case 2:
                    //MULTAS
                    try
                    {
                        var multOriginal = (ActMulta)new MultaRepository().OperacionesMultas(5, Id, 0);
                        if (multOriginal != null)
                        {
                            actMulta.Id = multOriginal.Id;
                            actMulta.IdMult = multOriginal.IdMult;
                            actMulta.IdUser = multOriginal.IdUser;
                            actMulta.FechaGeneracion = multOriginal.FechaGeneracion;
                            actMulta.Cuadrante = multOriginal.Cuadrante;
                            actMulta.Razon = multOriginal.Razon;
                            actMulta.Valor = multOriginal.Valor;
                            actMulta.Estado = multOriginal.Estado;
                            actMulta.FechaPago = multOriginal.FechaPago;
                            actMulta.CBancoOrigen = multOriginal.CBancoOrigen;
                            actMulta.NBancoOrigen = multOriginal.NBancoOrigen;
                            actMulta.CBancoDestino = multOriginal.CBancoDestino;
                            actMulta.NBancoDestino = multOriginal.NBancoDestino;
                            actMulta.CapturaPantalla = multOriginal.CapturaPantalla + IdCapturaPantalla.ToString() + ",";
                            _context.Update(actMulta);
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine("Hubo un problema al actualizar el campo de Captura de Pantalla de Multa.");
                        Console.WriteLine("Detalles del error: " + ex.Message);
                    }
                    break;
                default:
                    Console.WriteLine("opcion Inexistente");
                    break;
            }
        }
    }

}
