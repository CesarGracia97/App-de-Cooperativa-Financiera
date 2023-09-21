using act_Application.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TablaMultasController : Controller
    {
        [Authorize(Policy = "AdminSocioOnly")]
        public IActionResult Index()
        {
            var metodoMultas = new MetodoMultas();
            var multas = metodoMultas.ObtenerMultas();
            return View(multas);
        }
    }
}
