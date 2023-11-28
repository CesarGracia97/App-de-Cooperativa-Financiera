using act_Application.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TablaMultadosController : Controller
    {
        [Authorize(Policy = "AdminSocioOnly")]
        public IActionResult Index()
        {
            var mrobj = new MultaRepository().GetDataMultas();
            return View(mrobj);
        }
    }
}
