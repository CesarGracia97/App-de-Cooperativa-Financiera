using act_Application.Data.Data;
using act_Application.Logica.ComplementosLogicos;
using act_Application.Models.BD;
using act_Application.Models.Sistema;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class AportarController : Controller
    {

        private readonly DesarrolloContext _context;            //Variable conectora de Contexto


        public AportarController(DesarrolloContext context)     //Variable metodo de generacion de contexto.
        {
            _context = context;
        }

        private List<AportarItem> ObtenerItemsNBanco()          //Contenido de la Lista Bancos
        {
            return new List<AportarItem>
            {
                new AportarItem { Id = 1, Nombre = "Banco Pichincha" },
                new AportarItem { Id = 2, Nombre = "Banco Guayaquil" },
                new AportarItem { Id = 3, Nombre = "Produbanco" },
                new AportarItem { Id = 4, Nombre = "Banco del Austro" },
                new AportarItem { Id = 5, Nombre = "Banco Internacional" }
            };
        }

        private List<AportarItem> ObtenerItemsRazon()           //Contenido de la Lista Razones
        {
            return new List<AportarItem>
            {
                new AportarItem { Id = 1, Nombre = "CUOTA" }
            };
        }

        public IActionResult Aportar()                          //Muestra la vista
        {
            ViewData["ItemsNBanco"] = ObtenerItemsNBanco();
            ViewData["ItemsRazon"] = ObtenerItemsRazon();

            return View();
        }

        // POST: Rear Registro de Aportacion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aportar([Bind("Id,Razon,Valor,IdUser,FechaAportacion,Aprobacion,CapturaPantalla,Cuadrante1,Cuadrante2,Cbancaria,Nbanco")] ActAportacione actAportacione)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Establecer las propiedades que deben agregarse automáticamente
                    actAportacione.IdUser = userId;
                    actAportacione.FechaAportacion = DateTime.Now;
                    actAportacione.Aprobacion = "A la espera de la Aprobacion";
                    ObtenerCuadrante.CalcularCuadrantesAportacione(actAportacione);
                    _context.Add(actAportacione);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                    // Manejar el caso en que no se pueda obtener el Id del usuario
                    ModelState.AddModelError("", "Error al obtener el Id del usuario.");
                    Console.WriteLine("Fallo el guardado");
                }

            }
            return View(actAportacione);

        }


        /*
        public readonly DesarrolloContext _dbContext; 

        public AportarController(DesarrolloContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Aportar()
        {
            var viewModel = new AportarViewModel
            {
                Aportacion = new ActAportacione(), // Inicializar el modelo de aportación

                //Lista de Bancos
                ItemsNBanco = new List<AportarItem> 
                {
                    new AportarItem { Id = 1, Nombre = "Banco Pichincha" },
                    new AportarItem { Id = 2, Nombre = "Banco Guayaquil" },
                    new AportarItem { Id = 3, Nombre = "Produbanco" },
                    new AportarItem { Id = 4, Nombre = "Banco del Austro" },
                    new AportarItem { Id = 5, Nombre = "Banco Internacional" }
                },

                //Lista de Razones
                ItemsRazon = new List<AportarItem>
                {
                    new AportarItem { Id = 1, Nombre = "CUOTA" }
                }
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AportarPost([Bind("Id,Razon,Valor,IdUser,FechaAportacion,Aprobacion,CapturaPantalla,Cbancaria,Nbanco")] ActAportacione actAportacione)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Establecer las propiedades que deben agregarse automáticamente
                    actAportacione.IdUser = userId;
                    actAportacione.FechaAportacion = DateTime.Now;
                    actAportacione.Aprobacion = "A la espera de la Aprobacion";
                    ObtenerCuadrante.CalcularCuadrantesAportacione(actAportacione);
                    _dbContext.Add(actAportacione);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                    // Manejar el caso en que no se pueda obtener el Id del usuario
                    ModelState.AddModelError("", "Error al obtener el Id del usuario.");
                    Console.WriteLine("Fallo el guardado");
                }

            }
            return View(actAportacione);
        }

         */

    }
}