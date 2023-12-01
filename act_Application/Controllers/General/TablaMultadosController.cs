using act_Application.Data.Repository;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TablaMultadosController : Controller
    {
        [Authorize(Policy = "AdminSocioOnly")]
        public IActionResult Index()
        {
            var mobj = (List<ActMulta>)new MultaRepository().OperacionesMulta(3, 0, 0);
            return View(mobj);
        }
    }
}
