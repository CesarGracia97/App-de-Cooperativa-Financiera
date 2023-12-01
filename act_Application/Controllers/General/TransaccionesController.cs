using act_Application.Data;
using act_Application.Data.Context;
using act_Application.Data.Repository;
using act_Application.Logic.ComplementosLogicos;
using act_Application.Models.BD;
using act_Application.Services.ServiciosAplicativos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace act_Application.Controllers.General
{
    public class TransaccionesController : Controller
    {
        private readonly ActDesarrolloContext _context;
        private readonly NotificacionesServices _nservices;
        private readonly CapturaDePantallaServices _cpservices;
        public TransaccionesController(ActDesarrolloContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Aporte(decimal Valor, string NBancoOrigen, string CBancoOrigen, string NBancoDestino, string CBancoDestino, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdApor,IdUser,FechaAportacion,Cuadrante,Valor,NBancoOrigen,CBancoOrigen,NBancoDestino,CBancoDestino,CapturaPantalla,Estado")] ActAportacione actAportacione)
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
                    actAportacione.NBancoDestino = NBancoDestino;
                    actAportacione.CBancoDestino = CBancoDestino;
                    if (CapturaPantalla != null && CapturaPantalla.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await CapturaPantalla.CopyToAsync(ms);
                            var bytes = ms.ToArray();
                            actAportacione.CapturaPantalla = bytes; // Asigna los bytes de la imagen a la propiedad CapturaPantalla
                        }
                    }
                    _context.Add(actAportacione);
                    await _context.SaveChangesAsync();
                    string DescripcionA = $"El Usuario {userIdentificacion} (Usuario Id {IdUser}) a realizado un Aporte de {actAportacione.Valor} el dia {actAportacione.FechaAportacion}.";
                    string DescripcionU = $"Haz  realizado un Aporte de {actAportacione.Valor} el dia {actAportacione.FechaAportacion}.";
                    await _nservices.CrearNotificacion( 2, IdUser, (string) new AportacionRepository().OperacionesAportaciones(5,0,IdUser),"Aporte", DescripcionA,"ADMINISTRADOR", new ActNotificacione());
                    var essA = new EmailSendServices().EnviarCorreoAdmin(2, DescripcionA);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(actAportacione);
        }
        public async Task<IActionResult> PagoCuota(int Id, decimal Valor, string CBancoOrigen, string NBancoOrigen,string CBancoDestino, string NBancoDestino, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdCuot,IdUser,IdPrestamo,FechaCuota,Valor,Estado,FechaPago,CBancoOrigen,NBancoOrigen,CBancoDestino,NBancoDestino,HistorialValores,CapturaPantalla")] ActCuota actCuota)
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
                        var obj = new CuotaRepository();
                        var cuotOriginal = obj.GetIdDataCuotaUser(Id);
                        if (cuotOriginal == null)
                        {
                            return RedirectToAction("Error", "Home");
                        }

                        string DescripcionA = string.Empty, DescripcionU = string.Empty;

                        actCuota.IdUser = IdUser;
                        actCuota.IdPrestamo = cuotOriginal.IdPrestamo;
                        actCuota.FechaGeneracion = cuotOriginal.FechaGeneracion;
                        actCuota.FechaCuota = cuotOriginal.FechaCuota;
                        if (cuotOriginal.Valor - Valor <= 0)
                        {
                            actCuota.Valor = (cuotOriginal.Valor - Valor);
                            actCuota.Estado = "CUOTA CANCELADA";
                            actCuota.FechaPago =    cuotOriginal.FechaPago + DateTime.Now.ToString();
                            actCuota.CBancoOrigen = cuotOriginal.CBancoOrigen + CBancoOrigen;
                            actCuota.NBancoOrigen = cuotOriginal.NBancoOrigen + NBancoOrigen;
                            actCuota.CBancoDestino = cuotOriginal.CBancoDestino + CBancoDestino;
                            actCuota.NBancoDestino = cuotOriginal.NBancoDestino + NBancoDestino;
                            actCuota.HistorialValores = cuotOriginal.HistorialValores +  Valor.ToString();

                            DescripcionA = $"El Usuario {userIdentificacion} a Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La CUOTA a sido PAGADA COMPLETAMENTE (CANCELADA).";
                            DescripcionU = $"Haz Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La CUOTA a sido PAGADA COMPLETAMENTE (CANCELADA).";

                            var essA = new EmailSendServices().EnviarCorreoAdmin( 5, DescripcionA);
                            var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 5, DescripcionU);
                        }
                        else if (cuotOriginal.Valor - Valor > 0)
                        {
                            actCuota.Valor = cuotOriginal.Valor - Valor;
                            actCuota.Estado = cuotOriginal.Estado;
                            actCuota.FechaPago = cuotOriginal.FechaPago + DateTime.Now.ToString() + ",";
                            actCuota.CBancoOrigen = cuotOriginal.CBancoOrigen + CBancoOrigen + ",";
                            actCuota.NBancoOrigen = cuotOriginal.NBancoOrigen + NBancoOrigen + ",";
                            actCuota.CBancoDestino = cuotOriginal.CBancoDestino + CBancoDestino + ",";
                            actCuota.NBancoDestino = cuotOriginal.NBancoDestino + NBancoDestino + ",";
                            actCuota.HistorialValores = cuotOriginal.HistorialValores + Valor.ToString() + ",";

                            DescripcionA = $"El Usuario {userIdentificacion} a Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}, dejando un valor residual de ${cuotOriginal.Valor - Valor}. La CUOTA sigue estando PENDIENTE. ";
                            DescripcionU = $"Haz Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}, dejando un valor residual de ${cuotOriginal.Valor - Valor}. La CUOTA sigue estando PENDIENTE. ";
                            var essA = new EmailSendServices().EnviarCorreoAdmin( 6, DescripcionA);
                            var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 4, DescripcionU);
                        }
                        _context.Update(actCuota);
                        await _context.SaveChangesAsync();
                        await _nservices.CrearNotificacion( 3, IdUser, cuotOriginal.IdCuot, "PAGO DE CUOTA", DescripcionA, "ADMINISTRADOR", new ActNotificacione());
                        await _cpservices.SubirCapturaDePantalla( IdUser, "act_Cuotas", Id, CapturaPantalla, new ActCapturasPantalla());
                        int capobj = (int) new CapturaPantallaRepository().OperacionesCapPan( 1, 0, IdUser);
                        await _cpservices.ActualizarIdCapturaPantallaUser( 1, Id, capobj, new ActCuota(), new ActMulta());

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
        public async Task<IActionResult> Prestamo(decimal Valor, DateTime FechaEntrgaDinero, DateTime FechaInicioPagoCuotas, string  TipoCuota, [Bind("Id,IdPres,IdUser,Valor,FechaGeneracion,FechaEntregaDinero,FechaInicioPagoCuotas,FechaPagoTotalPrestamo,TipoCuota,Estado")] ActPrestamo actPrestamo)
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
                        actPrestamo.FechaEntregaDinero = FechaEntrgaDinero;
                        actPrestamo.FechaInicioPagoCuotas = FechaInicioPagoCuotas;
                        actPrestamo.FechaPagoTotalPrestamo = DateTime.MinValue;
                        actPrestamo.TipoCuota = TipoCuota;
                        actPrestamo.Estado = "PENDIENTE A";
                        _context.Add(actPrestamo);
                        await _context.SaveChangesAsync();
                        DescripcionA = $"El usuario {userIdentificacion} con C.I. {userCI} esta solicitando un prestamo de $ {actPrestamo.Valor} USD," +
                                                $"con fecha de entrega para el dia {actPrestamo.FechaEntregaDinero}, e inicio de pago de la deuda para el dia {actPrestamo.FechaInicioPagoCuotas}\n" +
                                                $"Estado: {actPrestamo.Estado}\n" + 
                                                $"Tipo de Cuota: {actPrestamo.TipoCuota}";
                        DescripcionU = $"Haz solicitado un prestamo de $ {actPrestamo.Valor} USD, con fecha de entrega para el dia {actPrestamo.FechaEntregaDinero}," +
                                                $" e inicio de pago de la deuda para el dia {actPrestamo.FechaInicioPagoCuotas}" +
                                                $"\nEstado: {actPrestamo.Estado}" +
                                                $"\nTipo de Cuota: {actPrestamo.TipoCuota}";
                        PrestamosRepository pobj = new PrestamosRepository();
                        await _nservices.CrearNotificacion(4, IdUser, pobj.H_GetLastIdPres(IdUser), "PETICION DE PRESTAMO", DescripcionA, "ADMINISTRADOR", new ActNotificacione());
                        var essA = new EmailSendServices().EnviarCorreoAdmin(7, DescripcionA);
                        var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 1, DescripcionU);
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
        public async Task<IActionResult> PagoMulta(int Id, decimal Valor, string CBancoOrigen, string NBancoOrigen, string CBancoDestino, string NBancoDestino, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdMult,IdUser,FechaGeneracion,Cuadrante,Razon,Valor,Estado,FechaPago,CBancoOrigen,NBancoOrigen,CBancoDestino,NBancoDestino,HistorialValores,CapturaPantalla")]ActMulta actMulta)
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
                        var multOriginal = (ActMulta) new MultaRepository().OperacionesMulta(5, Id, 0);
                        if (multOriginal == null)
                            return RedirectToAction("Error", "Home");

                        string DescripcionA = string.Empty, DescripcionU = string.Empty;

                        actMulta.IdMult = multOriginal.IdMult;
                        actMulta.IdUser = multOriginal.IdUser;
                        actMulta.FechaGeneracion = multOriginal.FechaGeneracion;
                        actMulta.Cuadrante = multOriginal.Cuadrante;
                        actMulta.Razon = multOriginal.Razon;
                        if(multOriginal.Valor - Valor <= 0)
                        {
                            actMulta.Valor = (multOriginal.Valor - Valor);
                            actMulta.Estado = "MULTA CANCELADA";
                            actMulta.FechaPago = multOriginal.FechaPago + DateTime.Now.ToString();
                            actMulta.NBancoOrigen = multOriginal.NBancoOrigen + NBancoOrigen;
                            actMulta.CBancoOrigen = multOriginal.CBancoOrigen + CBancoOrigen;
                            actMulta.NBancoDestino = multOriginal.NBancoDestino + NBancoDestino;
                            actMulta.CBancoDestino = multOriginal.CBancoDestino + CBancoDestino + ",";
                            actMulta.HistorialValores = multOriginal.HistorialValores + Valor.ToString();

                            DescripcionA = $"El Usuario {userIdentificacion} a Realizado un PAGO DE MULTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La MULTA a sido PAGADA COMPLETAMENTE (CANCELADA).";
                            DescripcionU = $"Haz  Realizado un PAGO DE MULTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La MULTA a sido PAGADA COMPLETAMENTE (CANCELADA)."; 
                            var essA = new EmailSendServices().EnviarCorreoAdmin(3, DescripcionA);
                            var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 5, DescripcionU);
                        }
                        else if (multOriginal.Valor - Valor > 0)
                        {
                            actMulta.Valor = (multOriginal.Valor - Valor);
                            actMulta.Estado = multOriginal.Estado;
                            actMulta.FechaPago = multOriginal.FechaPago + DateTime.Now.ToString();
                            actMulta.NBancoOrigen = multOriginal.NBancoOrigen + NBancoOrigen + ",";
                            actMulta.CBancoOrigen = multOriginal.CBancoOrigen + CBancoOrigen + ",";
                            actMulta.NBancoDestino = multOriginal.NBancoDestino + NBancoDestino + ",";
                            actMulta.CBancoDestino = multOriginal.CBancoDestino + CBancoDestino + ",";
                            actMulta.HistorialValores = multOriginal.HistorialValores + Valor.ToString() + ",";

                            DescripcionA = $"El Usuario {userIdentificacion} a Realizado un PAGO DE MULTA el dia {DateTime.Now}, cuyo valor es de ${Valor}, dejando un valor residual de ${multOriginal.Valor - Valor}. La MULTA sigue estando PENDIENTE. ";
                            DescripcionU = $"El Usuario {userIdentificacion} a Realizado un PAGO DE MULTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La MULTA a sido PAGADA COMPLETAMENTE (CANCELADA).";
                            var essA = new EmailSendServices().EnviarCorreoAdmin(6, DescripcionA);
                            var essU = new EmailSendServices().EnviarCorreoUsuario(IdUser, 4, DescripcionU);
                        }
                        _context.Update(actMulta);
                        await _context.SaveChangesAsync();

                        await _nservices.CrearNotificacion(5, IdUser, multOriginal.IdMult, "PAGO DE MULTA", DescripcionA, "ADMINISTRADOR", new ActNotificacione());
                        await _cpservices.SubirCapturaDePantalla(IdUser, "act_Multas", Id, CapturaPantalla, new ActCapturasPantalla());
                        int capobj = (int)new CapturaPantallaRepository().OperacionesCapPan( 1, 0, IdUser);
                        await _cpservices.ActualizarIdCapturaPantallaUser(2, Id, capobj, new ActCuota(), new ActMulta());
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
    }
}
