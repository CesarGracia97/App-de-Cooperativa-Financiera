using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class NotificacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
