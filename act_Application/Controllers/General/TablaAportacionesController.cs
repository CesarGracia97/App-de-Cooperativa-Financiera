using act_Application.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General  
{
    public class TablaAportacionesController : Controller
    {
        [Authorize(Policy = "AdminSocioOnly")]
        public IActionResult Index()
        {
            var metodoAportaciones = new MetodoAportaciones();
            var aportaciones = metodoAportaciones.ObtenerAportaciones();

            return View(aportaciones);
        }
    }
}
