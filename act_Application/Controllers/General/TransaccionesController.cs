using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TransaccionesController : Controller
    {
        public IActionResult Transaccion()
        {
            return View();
        }
        public async Task<IActionResult> Aporte([Bind("Id,IdApor,IdUser,Cuadrante,Valor,NBancoOrigen,CBancoOrigen,NBancoDestino,CBancoDestino,CapturaPantalla,Estado")]ActAportacione actAportacione)
        {
            return View(actAportacione);
        }
        public async Task<IActionResult> PagoCuota([Bind("Id,IdCuot,")]ActCuota actCuota)
        {
            return View(actCuota);
        }
        public async Task<IActionResult> Prestamo()
        {

        }
        public async Task<IActionResult> PagoMulta()
        {

        }
    }
}
