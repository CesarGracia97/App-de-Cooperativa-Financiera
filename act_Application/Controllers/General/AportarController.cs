using act_Application.Models.BD;
using act_Application.Models.Sistema;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Controllers.General
{
    public class AportarController : Controller
    {
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
    }
}
