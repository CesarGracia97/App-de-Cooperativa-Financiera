using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Logic.ComplementosLogicos;
using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using act_Application.Models.Sistema.ViewModel;
using act_Application.Services.ServiciosAplicativos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace act_Application.Controllers.General
{
    public class TransaccionesController : Controller
    {
        private readonly ActDesarrolloContext _context;
        private readonly ILogger<TransaccionesController> logger;
        public TransaccionesController(ActDesarrolloContext context, ILogger<TransaccionesController> logger)
        {
            _context = context;
            this.logger = logger;
        }
        private List<ListItems> ObtenerItemsCuota()
        {
            return new List<ListItems>
            {
                new ListItems { Id = 1, Nombre = "PAGO UNICO" },
                new ListItems { Id = 2, Nombre = "PAGO MENSUAL" }
            };
        }
        public IActionResult Index()
        {
            ViewData["ItemsCuota"] = ObtenerItemsCuota();
            var obj = (List<ActCuentaDestino>) new DestinoRepository().OperacionDestino(1, 0, 0);
            // Estructurar las listas para las opciones del banco destino
            var itemCuentaBancoDestino = obj.Select(cuenta =>
                new
                {
                    Value = $"{cuenta.NombreBanco} - #{cuenta.NumeroCuentaB}",
                    Text = $"{cuenta.NombreBanco} - #{cuenta.NumeroCuentaB}"
                }).ToList();

            var viewModel = new Transacciones_VM
            {
                ItemCuentaBancoDestino = obj,
                ItemCuotas = ObtenerItemsCuota()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> Aporte(decimal Valor, string NBancoOrigen, string CBancoOrigen, string CuentaDestino, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdApor,IdUser,FechaAportacion,Cuadrante,Valor,NBancoOrigen,CBancoOrigen,NBancoDestino,CBancoDestino,CapturaPantalla,Estado")] ActAportacione actAportacione)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                    {
                        //
                        var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        //
                        actAportacione.IdUser = IdUser;
                        actAportacione.FechaAportacion = DateTime.Now;
                        ObtenerCuadrante obj = new ObtenerCuadrante();
                        actAportacione.Cuadrante = obj.Cuadrante(DateTime.Now);
                        actAportacione.Valor = Valor;
                        actAportacione.NBancoOrigen = NBancoOrigen;
                        actAportacione.CBancoOrigen = CBancoOrigen;
                        if (!string.IsNullOrEmpty(CuentaDestino))
                        {
                            var parts = CuentaDestino.Split(" - #");
                            if (parts.Length == 2)
                            {
                                actAportacione.NBancoDestino = parts[0];
                                actAportacione.CBancoDestino = parts[1];
                            }
                        }
                        if (CapturaPantalla != null && CapturaPantalla.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                await CapturaPantalla.CopyToAsync(ms);
                                var bytes = ms.ToArray();
                                actAportacione.CapturaPantalla = bytes; // Asigna los bytes de la imagen a la propiedad CapturaPantalla
                            }
                        }
                        actAportacione.Estado = "ESPERA";
                        _context.Add(actAportacione);
                        await _context.SaveChangesAsync();
                        string DescripcionA = $"El Usuario {userIdentificacion} (Usuario Id {IdUser}) a realizado un Aporte de {actAportacione.Valor} el dia {actAportacione.FechaAportacion}.";
                        string DescripcionU = $"Haz  realizado un Aporte de {actAportacione.Valor} el dia {actAportacione.FechaAportacion}.";
                        string IdActividad = (string)new AportacionRepository().OperacionesAportaciones(5, 0, IdUser, "");
                        await new NotificacionesServices(_context).CrearNotificacion(2, 2, 0, IdUser, IdActividad, "Aporte", DescripcionA, "Administrador", new ActNotificacione());
                        var essA = new EmailSendServices().EnviarCorreoAdmin(2, DescripcionA);
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View(actAportacione);
            }
            catch (Exception ex)
            {
                logger.LogError("TransaccionesController. Error en Aportar: ", ex);
                return null;
            }
        }
        public async Task<IActionResult> Prestamo(decimal Valor, DateTime FechaEntregaDinero, string TipoCuota, [Bind("Id,IdPres,IdUser,Valor,FechaGeneracion,FechaEntregaDinero,FechaInicioPagoCuotas,FechaPagoTotalPrestamo,TipoCuota,Estado")] ActPrestamo actPrestamo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                    {
                        //
                        var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                        var userCI = User.Claims.FirstOrDefault(c => c.Type == "CI")?.Value;
                        //
                        string DescripcionA = string.Empty, DescripcionU = string.Empty;

                        actPrestamo.IdUser = IdUser;
                        actPrestamo.Valor = Valor;
                        actPrestamo.FechaGeneracion = DateTime.Now;
                        actPrestamo.FechaEntregaDinero = FechaEntregaDinero;
                        actPrestamo.FechaInicioPagoCuotas = DateTime.MinValue;
                        actPrestamo.FechaPagoTotalPrestamo = DateTime.MinValue;
                        actPrestamo.TipoCuota = TipoCuota;
                        actPrestamo.Estado = "PENDIENTE A";
                        _context.Add(actPrestamo);
                        await _context.SaveChangesAsync();
                        DescripcionA = $"El usuario {userIdentificacion} con C.I. {userCI} esta solicitando un prestamo de $ {actPrestamo.Valor} USD," +
                                                $"con fecha de entrega para el dia {actPrestamo.FechaEntregaDinero}\n" +
                                                $"Estado: {actPrestamo.Estado}\n" +
                                                $"Tipo de Cuota: {actPrestamo.TipoCuota}";
                        DescripcionU = $"Haz solicitado un prestamo de $ {actPrestamo.Valor} USD, con fecha de entrega para el dia {actPrestamo.FechaEntregaDinero}." +
                                                $"\nEstado: {actPrestamo.Estado}" +
                                                $"\nTipo de Cuota: {actPrestamo.TipoCuota}";
                        await new NotificacionesServices(_context).CrearNotificacion(2, 4, 0, IdUser, (string)new PrestamosRepository().OperacionesPrestamos(5, 0, IdUser, ""), "PETICION DE PRESTAMO", DescripcionA, "Administrador", new ActNotificacione());
                        var essA = new EmailSendServices().EnviarCorreoAdmin(7, DescripcionA);
                        var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 1, DescripcionU);
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al crear la peticion de Prestamo.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
            }
            return View(actPrestamo);
        }
        public async Task<IActionResult> PagoMulta(int Id, decimal Valor, string CBancoOrigen, string NBancoOrigen, string CuentaDestino, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdMult,IdUser,FechaGeneracion,Cuadrante,Razon,Valor,Estado,FechaPago,CBancoOrigen,NBancoOrigen,CBancoDestino,NBancoDestino,HistorialValores,CapturaPantalla")] ActMulta actMulta)
        {
            try
            {
                if (Id != actMulta.Id)
                    return RedirectToAction("Error", "Home");
                if (ModelState.IsValid)
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                    {
                        //
                        var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        //
                        var multOriginal = (ActMulta)new MultaRepository().OperacionesMultas(5, Id, 0, "");
                        if (multOriginal == null)
                            return RedirectToAction("Error", "Home");

                        string DescripcionA = string.Empty, DescripcionU = string.Empty, Razon = string.Empty;
                        await new CapturaDePantallaServices(_context).SubirCapturaDePantalla(IdUser, "act_Multas", Id, CapturaPantalla, new ActCapturasPantalla());
                        actMulta.IdUser = multOriginal.IdUser;
                        actMulta.FechaGeneracion = multOriginal.FechaGeneracion;
                        actMulta.Cuadrante = multOriginal.Cuadrante;
                        actMulta.Razon = multOriginal.Razon;
                        if (multOriginal.Valor - Valor <= 0)
                        {
                            actMulta.Valor = (multOriginal.Valor - Valor);
                            actMulta.IdMult = "CMUL-" + Id;
                            actMulta.Estado = "MULTA CANCELADA";
                            actMulta.FechaPago = multOriginal.FechaPago + DateTime.Now.ToString();
                            actMulta.NBancoOrigen = multOriginal.NBancoOrigen + NBancoOrigen;
                            actMulta.CBancoOrigen = multOriginal.CBancoOrigen + CBancoOrigen;
                            if (!string.IsNullOrEmpty(CuentaDestino))
                            {
                                var parts = CuentaDestino.Split(" - #");
                                if (parts.Length == 2)
                                {
                                    actMulta.NBancoDestino = multOriginal.NBancoDestino + parts[0];
                                    actMulta.CBancoDestino = multOriginal.CBancoDestino + parts[1];
                                }
                            }
                            actMulta.HistorialValores = multOriginal.HistorialValores + Valor.ToString();
                            int capobj = (int) new CapturaPantallaRepository().OperacionesCapPan(1, 0, IdUser);
                            actMulta.CapturaPantalla =multOriginal.CapturaPantalla + capobj.ToString();

                            Razon = "PAGO DE MULTA";
                            DescripcionA = $"El Usuario {userIdentificacion} a Realizado un PAGO DE MULTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La MULTA a sido PAGADA COMPLETAMENTE (CANCELADA).";
                            DescripcionU = $"Haz  Realizado un PAGO DE MULTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La MULTA a sido PAGADA COMPLETAMENTE (CANCELADA).";
                            var essA = new EmailSendServices().EnviarCorreoAdmin(3, DescripcionA);
                            var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 5, DescripcionU);
                        }
                        else if (multOriginal.Valor - Valor > 0)
                        {
                            actMulta.IdMult = "AMUL-" + Id;
                            actMulta.Valor = (multOriginal.Valor - Valor);
                            actMulta.Estado = multOriginal.Estado;
                            actMulta.FechaPago = multOriginal.FechaPago + DateTime.Now.ToString();
                            actMulta.NBancoOrigen = multOriginal.NBancoOrigen + NBancoOrigen + ",";
                            actMulta.CBancoOrigen = multOriginal.CBancoOrigen + CBancoOrigen + ",";
                            if (!string.IsNullOrEmpty(CuentaDestino))
                            {
                                var parts = CuentaDestino.Split(" - #");
                                if (parts.Length == 2)
                                {
                                    actMulta.NBancoDestino = multOriginal.NBancoDestino + parts[0] + ",";
                                    actMulta.CBancoDestino = multOriginal.CBancoDestino + parts[1] + ",";
                                }
                            }
                            actMulta.HistorialValores = multOriginal.HistorialValores + Valor.ToString() + ",";
                            int capobj = (int) new CapturaPantallaRepository().OperacionesCapPan(1, 0, IdUser);
                            actMulta.CapturaPantalla = multOriginal.CapturaPantalla + capobj.ToString() + ",";

                            Razon = "ABONO DE MULTA";
                            DescripcionA = $"El Usuario {userIdentificacion} a Realizado un PAGO DE MULTA el dia {DateTime.Now}, cuyo valor es de ${Valor}, dejando un valor residual de ${multOriginal.Valor - Valor}. La MULTA sigue estando PENDIENTE. ";
                            DescripcionU = $"El Usuario {userIdentificacion} a Realizado un PAGO DE MULTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La MULTA a sido PAGADA COMPLETAMENTE (CANCELADA).";
                            var essA = new EmailSendServices().EnviarCorreoAdmin(6, DescripcionA);
                            var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 4, DescripcionU);
                        }
                        _context.Update(actMulta);
                        await _context.SaveChangesAsync();

                        await new NotificacionesServices(_context).CrearNotificacion(2, 5, 0, IdUser, actMulta.IdMult, Razon, DescripcionA, "Administrador", new ActNotificacione());
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("Hubo un problema al realizar el Pago de la Multa.");
                Console.WriteLine("Detalles del error: " + ex.Message);
            }
            return View(actMulta);
        }
        public async Task<IActionResult> PagoCuota(int Id, decimal Valor, string CBancoOrigen, string NBancoOrigen, string CuentaDestino, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdCuot,IdUser,IdPrestamo,FechaCuota,Valor,Estado,FechaPago,CBancoOrigen,NBancoOrigen,CBancoDestino,NBancoDestino,HistorialValores,CapturaPantalla")] ActCuota actCuota)
        {
            if (Id != actCuota.Id)
            {
                return RedirectToAction("Error", "Home");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int IdUser))
                    {
                        //
                        var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        //
                        var cuotOriginal = (ActCuota) new CuotaRepository().OperacionesCuotas( 2, Id, 0, "");
                        if (cuotOriginal == null)
                        {
                            return RedirectToAction("Error", "Home");
                        }
                        string DescripcionA = string.Empty, DescripcionU = string.Empty, Razon =string.Empty;
                        await new CapturaDePantallaServices(_context).SubirCapturaDePantalla(IdUser, "act_Cuotas", Id, CapturaPantalla, new ActCapturasPantalla());
                        actCuota.IdUser = IdUser;
                        actCuota.IdPrestamo = cuotOriginal.IdPrestamo;
                        actCuota.FechaGeneracion = cuotOriginal.FechaGeneracion;
                        actCuota.FechaCuota = cuotOriginal.FechaCuota;
                        if (cuotOriginal.Valor - Valor <= 0)
                        {
                            actCuota.Valor = (cuotOriginal.Valor - Valor);
                            actCuota.Estado = "CUOTA CANCELADA";
                            actCuota.IdCuot = "CCUO-" + Id;
                            actCuota.FechaPago =    cuotOriginal.FechaPago + DateTime.Now.ToString();
                            actCuota.CBancoOrigen = cuotOriginal.CBancoOrigen + CBancoOrigen;
                            actCuota.NBancoOrigen = cuotOriginal.NBancoOrigen + NBancoOrigen;
                            if (!string.IsNullOrEmpty(CuentaDestino))
                            {
                                var parts = CuentaDestino.Split(" - #");
                                if (parts.Length == 2)
                                {
                                    actCuota.NBancoDestino = cuotOriginal.NBancoDestino + parts[0] + ",";
                                    actCuota.CBancoDestino = cuotOriginal.CBancoDestino + parts[1] + ",";
                                }
                            }
                            actCuota.HistorialValores = cuotOriginal.HistorialValores +  Valor.ToString();
                            int capobj = (int)new CapturaPantallaRepository().OperacionesCapPan(1, 0, IdUser);
                            actCuota.CapturaPantalla = cuotOriginal.CapturaPantalla + capobj.ToString();

                            Razon = "PAGO DE CUOTA";
                            DescripcionA = $"El Usuario {userIdentificacion} a Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La CUOTA a sido PAGADA COMPLETAMENTE (CANCELADA).";
                            DescripcionU = $"Haz Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La CUOTA a sido PAGADA COMPLETAMENTE (CANCELADA).";

                            var essA = new EmailSendServices().EnviarCorreoAdmin( 5, DescripcionA);
                            var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 5, DescripcionU);
                        }
                        else if (cuotOriginal.Valor - Valor > 0)
                        {
                            actCuota.IdCuot = "ACUO-" + Id;
                            actCuota.Valor = cuotOriginal.Valor - Valor;
                            actCuota.Estado = cuotOriginal.Estado;
                            actCuota.FechaPago = cuotOriginal.FechaPago + DateTime.Now.ToString() + ",";
                            actCuota.CBancoOrigen = cuotOriginal.CBancoOrigen + CBancoOrigen + ",";
                            actCuota.NBancoOrigen = cuotOriginal.NBancoOrigen + NBancoOrigen + ",";
                            if (!string.IsNullOrEmpty(CuentaDestino))
                            {
                                var parts = CuentaDestino.Split(" - #");
                                if (parts.Length == 2)
                                {
                                    actCuota.NBancoDestino = cuotOriginal.NBancoDestino + parts[0] + ",";
                                    actCuota.CBancoDestino = cuotOriginal.CBancoDestino + parts[1] + ",";
                                }
                            }
                            actCuota.HistorialValores = cuotOriginal.HistorialValores + Valor.ToString() + ",";
                            int capobj = (int)new CapturaPantallaRepository().OperacionesCapPan(1, 0, IdUser);
                            actCuota.CapturaPantalla = cuotOriginal.CapturaPantalla + capobj.ToString() + ",";

                            Razon = "ABONO DE CUOTA";
                            DescripcionA = $"El Usuario {userIdentificacion} a Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}, dejando un valor residual de ${cuotOriginal.Valor - Valor}. La CUOTA sigue estando PENDIENTE. ";
                            DescripcionU = $"Haz Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}, dejando un valor residual de ${cuotOriginal.Valor - Valor}. La CUOTA sigue estando PENDIENTE. ";
                            var essA = new EmailSendServices().EnviarCorreoAdmin( 6, DescripcionA);
                            var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 4, DescripcionU);
                        }
                        _context.Update(actCuota);
                        await _context.SaveChangesAsync();
                        await new NotificacionesServices(_context).CrearNotificacion(2, 3, 0, IdUser, actCuota.IdCuot, Razon, DescripcionA, "Administrador", new ActNotificacione());
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Hubo un problema al actualizar el registro del pago de la Cuota.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
            }
            return View(actCuota);
        }
    }
}
