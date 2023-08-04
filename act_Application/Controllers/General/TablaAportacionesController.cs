using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TablaAportacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
