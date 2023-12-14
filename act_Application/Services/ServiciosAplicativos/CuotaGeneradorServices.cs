using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Services.ServiciosAplicativos
{
    public class CuotaGeneradorServices
    {
        private readonly ActDesarrolloContext _context;

        public CuotaGeneradorServices(ActDesarrolloContext context)
        {
            _context = context;
        }
        public async Task<List<DateTime>> CrearCuotas(int Id, DateTime FechaPagoTotalPrestamo, [Bind("Id,IdCuot,IdUser,IdPrestamo,FechaGeneracion,FechaCuota,Valor,Estado")] ActCuota actCuota)
        {
            var pobj = (ActPrestamo) new PrestamosRepository().OperacionesPrestamos(2, Id, 0, "");
            if (pobj == null)
            {
                Console.WriteLine("CuotaGeneradorServices - CrearCuotas | Existe un problema con el Prestamo.");
            }
            int NCuotas = (FechaPagoTotalPrestamo.Year - pobj.FechaInicioPagoCuotas.Year) * 12 + FechaPagoTotalPrestamo.Month - pobj.FechaInicioPagoCuotas.Month + 1;
            decimal ValorCuota = pobj.Valor / NCuotas;

            var fechasDeCuotas = new List<DateTime>();

            for (int i = 0; i < NCuotas; i++)
            {
                DateTime fechaCuota = pobj.FechaInicioPagoCuotas.AddMonths(i);
                fechasDeCuotas.Add(fechaCuota);

                var cuota = new ActCuota
                {
                    IdUser = pobj.IdUser,
                    IdPrestamo = Id,
                    Valor = ValorCuota,
                    FechaCuota = fechaCuota,
                    Estado = "PENDIENTE"
                };

                _context.Add(cuota);
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return fechasDeCuotas;
        }
    }
}
