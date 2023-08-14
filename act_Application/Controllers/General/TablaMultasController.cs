using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TablaMultasController : Controller
    {
        [Authorize(Policy = "AdminSocioOnly")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
