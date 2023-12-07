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
            List<ActMulta> mobj = null;
            if ((bool)new MultaRepository().OperacionesMultas(1, 0, 0))
            {
                mobj = (List<ActMulta>) new MultaRepository().OperacionesMultas(3, 0, 0);
            }
            return View(mobj);
        }
    }
}
