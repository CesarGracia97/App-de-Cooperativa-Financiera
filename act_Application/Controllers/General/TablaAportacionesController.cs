using act_Application.Logica;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TablaAportacionesController : Controller
    {
        public IActionResult Index()
        {
            var metodoAportaciones = new MetodoAportaciones();
            var aportaciones = metodoAportaciones.ObtenerAportaciones();

            return View(aportaciones);
        }
    }
}
