using act_Application.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class TablaAportantesController : Controller
    {
        public IActionResult Index()
        {
            var arobj = new AportacionRepository().GetDataAportaciones();
            return View(arobj);
        }
    }
}
