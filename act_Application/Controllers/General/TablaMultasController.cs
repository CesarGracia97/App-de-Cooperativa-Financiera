using act_Application.Data.Data;
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
            var multaRepository = new MultaRepository();
            var multas = multaRepository.GetDataMultas();
            return View(multas);
        }
    }
}
