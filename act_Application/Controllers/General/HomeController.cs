using act_Application.Data.Context;
using act_Application.Data.Data;
using act_Application.Models;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace act_Application.Controllers.General
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ActDesarrolloContext _context;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public HomeController (ActDesarrolloContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Participar(int Id, [Bind("Id,IdEven,IdPrestamo,IdUser,ParticiantesId,NombresPId,FechaGeneracion,FechaInicio,FechaFinalizacion,Estado")] ActEvento actEvento)
        {
            if(Id != actEvento.Id)
            {
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var eobj = new EventosRepository().GetDataEventoPorId(Id);

                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                    {
                        //
                        var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        //
                        actEvento.IdEven = eobj.IdEven;
                        actEvento.IdPrestamo = eobj.IdPrestamo;
                        actEvento.IdUser = IdUser;
                        actEvento.ParticipantesId = eobj.ParticipantesId + IdUser.ToString() + ",";
                        actEvento.NombresPId = eobj.NombresPId + userIdentificacion + ",";
                        actEvento.FechaGeneracion = eobj.FechaGeneracion;
                        actEvento.FechaInicio = eobj.FechaInicio;
                        actEvento.FechaFinalizacion = eobj.FechaFinalizacion;
                        actEvento.Estado = eobj.Estado;
                    }
                    _context.Update(actEvento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al actualizar el registro del pago de la Cuota.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
            }
            return View(actEvento);
        }
    }
}