using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class AporController : Controller
    {
        // GET: AporController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AporController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AporController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AporController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AporController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AporController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AporController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AporController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
