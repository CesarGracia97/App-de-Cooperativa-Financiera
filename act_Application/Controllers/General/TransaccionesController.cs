using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TransaccionesController : Controller
    {
        public IActionResult Transaccion()
        {
            return View();
        }
    }
}
