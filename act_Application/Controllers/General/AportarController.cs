using Microsoft.AspNetCore.Mvc;
using act_Application.Models.BD;
using act_Application.Data.Data;
using act_Application.Models.Sistema.ViewModels;
using act_Application.Logic.ComplementosLogicos;
using Microsoft.AspNetCore.Authorization;
using act_Application.Logic;
using act_Application.Models.Sistema.Complementos;

namespace act_Application.Controllers.General
{
    public class AportarController : Controller
    {
        private readonly DesarrolloContext _context;

        public AportarController(DesarrolloContext context)
        {
            _context = context;
        }
        private List<ListItems> ObtenerItemsNBanco()          //Contenido de la Lista Bancos
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "Banco Pichincha" },
                new ListItems { Id = 2, Nombre = "Banco Guayaquil" },
                new ListItems { Id = 3, Nombre = "Produbanco" },
                new ListItems { Id = 4, Nombre = "Banco del Austro" },
                new ListItems { Id = 5, Nombre = "Banco Internacional" }
            };
        }

        private List<ListItems> ObtenerItemsRazon()           //Contenido de la Lista Razones
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "CUOTA" }
            };
        }

        // GET: Aportar/Create
        [Authorize(Policy = "AdminSocioOnly")]
        public IActionResult Aportar()
        {
            ViewData["ItemsNBanco"] = ObtenerItemsNBanco();
            ViewData["ItemsRazon"] = ObtenerItemsRazon();

            MetodoDestino.ObetnerDestinos obetnerDestinos = new MetodoDestino.ObetnerDestinos();
            List<ActCuentaDestino> cbDestinos = obetnerDestinos.ObtenerTodosLosRegistros();

            // Estructurar las listas para las opciones del banco destino
            var itemCuentaBancoDestino = cbDestinos.Select(cuenta =>
                new
                {
                    Value = $"{cuenta.NombreBanco} - #{cuenta.NumeroCuenta}",
                    Text = $"{cuenta.NombreBanco} - #{cuenta.NumeroCuenta}"
                }).ToList();

            var viewModel = new Aportar_VM
            {
                ItemCuentaBancoDestino = cbDestinos
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminSocioOnly")]
        public async Task<IActionResult> Aportacion([Bind("Id,Razon,Valor,IdUser,FechaAportacion,Aprobacion,CapturaPantalla,Cuadrante1,Cuadrante2,Nbanco,Cbancaria,CuentaDestino,BancoDestino")] ActAportacione actAportacione, [FromForm] IFormFile imagen, string Cuentadestino, string Cbancaria, string Nbanco, decimal Valor)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Establecer las propiedades que deben agregarse automáticamente
                    actAportacione.IdUser = userId;
                    actAportacione.FechaAportacion = DateTime.Now;
                    actAportacione.Aprobacion = "EN ESPERA";
                    actAportacione.Cbancaria = Cbancaria;
                    actAportacione.Nbanco = Nbanco;
                    actAportacione.Valor = Valor;
                    ObtenerCuadrante.CalcularCuadrantesAportacione(actAportacione);

                    if (!string.IsNullOrEmpty(Cuentadestino))
                    {
                        var parts = Cuentadestino.Split(" - #");
                        if (parts.Length == 2)
                        {
                            actAportacione.BancoDestino = parts[0];
                            actAportacione.CuentaDestino = parts[1];
                        }
                    }


                    if (imagen != null && imagen.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await imagen.CopyToAsync(ms);
                            var bytes = ms.ToArray();
                            actAportacione.CapturaPantalla = bytes; // Asigna los bytes de la imagen a la propiedad CapturaPantalla
                        }
                    }

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

        private bool ActAportacioneExists(int id)
        {
          return (_context.ActAportaciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
