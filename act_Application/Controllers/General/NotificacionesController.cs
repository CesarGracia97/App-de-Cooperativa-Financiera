using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class NotificacionesController : Controller
    {
        [Authorize(Policy = "AllOnly")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
