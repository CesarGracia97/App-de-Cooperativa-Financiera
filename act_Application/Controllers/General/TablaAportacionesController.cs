using act_Application.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General  
{
    public class TablaAportacionesController : Controller
    {
        [Authorize(Policy = "AdminSocioOnly")]
        public IActionResult Index()
        {
            var aportacionesRepository = new AportacionRepository();
            var aportaciones = aportacionesRepository.GetDataAportaciones();

            return View(aportaciones);
        }
    }
}
