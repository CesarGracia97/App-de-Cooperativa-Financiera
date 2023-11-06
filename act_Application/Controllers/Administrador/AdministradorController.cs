using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.Administrador
{
    public class AdministradorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
