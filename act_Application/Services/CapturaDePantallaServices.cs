using act_Application.Data.Context;
using act_Application.Data.Data;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace act_Application.Services
{
    public class CapturaDePantallaServices
    {
        private readonly ActDesarrolloContext _context;
        public CapturaDePantallaServices(ActDesarrolloContext context)
        {
            _context = context;
        }
        public async Task SubirCapturaDePantalla( int IdUser, string Origen, int IdOrigenCaptura, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdUser,Origen,IdOrigen,CapturaPantalla")] ActCapturasPantalla actCapturasPantalla)
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
        public async Task ActualizarIdCapturaPantallaUser(int Id, int IdCapturaPantalla, [Bind("Id,IdCuot,IdUser,IdPrestamo,FechaCuota,Valor,Estado,FechaPago,CBancoOrigen,NBancoOrigen,CBancoDestino,NBancoDestino,HistorialValores,CapturaPantalla")] ActCuota actCuota)
        {
            try
            {
                var obj = new CuotaRepository();
                var cuotOriginal = obj.GetIdDataCuotaUser(Id);
                if (cuotOriginal == null)
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

                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("Hubo un problema al actualizar el campo de Captura de Pantalla de Cuota");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
        }
    }
}
