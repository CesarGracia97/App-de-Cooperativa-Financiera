﻿using act_Application.Data;
using act_Application.Data.Context;
using act_Application.Data.Data;
using act_Application.Logic.ComplementosLogicos;
using act_Application.Models.BD;
using act_Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

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
        public IActionResult Transaccion()
        {
            return View();
        }
        public async Task<IActionResult> Aporte(decimal Valor, string NBancoOrigen, string CBancoOrigen, string NBancoDestino, string CBancoDestino, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdApor,IdUser,FechaAportacion,Cuadrante,Valor,NBancoOrigen,CBancoOrigen,NBancoDestino,CBancoDestino,CapturaPantalla,Estado")] ActAportacione actAportacione)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    //
                    var userIdentificacion = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    //
                    actAportacione.IdUser = userId; 
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
                    AportacionRepository aobj = new AportacionRepository();
                    string Descripcion = $"El Usuario {userIdentificacion} (Usuario Id {userId}) a realizado un Aporte de {actAportacione.Valor} el dia {actAportacione.FechaAportacion}.";
                    await _nservices.CrearNotificacion( 2, userId, aobj.H_GetLastIdApor(actAportacione.IdUser),"Aporte", Descripcion,"ADMINISTRADOR", new ActNotificacione());
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
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
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

                        string Descripcion = string.Empty;

                        actCuota.IdUser = userId;
                        actCuota.IdPrestamo = cuotOriginal.IdPrestamo;
                        actCuota.FechaGeneracion = cuotOriginal.FechaGeneracion;
                        actCuota.FechaCuota = cuotOriginal.FechaCuota;
                        if (cuotOriginal.Valor - Valor <= 0)
                        {
                            actCuota.Valor = 0;
                            actCuota.Estado = "CUOTA CANCELADA";
                            actCuota.FechaPago = DateTime.Now.ToString();
                            actCuota.CBancoOrigen = cuotOriginal.CBancoOrigen + CBancoOrigen;
                            actCuota.NBancoOrigen = cuotOriginal.NBancoOrigen + NBancoOrigen;
                            actCuota.CBancoDestino = cuotOriginal.CBancoDestino + CBancoDestino;
                            actCuota.NBancoDestino = cuotOriginal.NBancoDestino + NBancoDestino;
                            actCuota.HistorialValores = cuotOriginal.HistorialValores +  Valor.ToString();

                            Descripcion = $"El Usuario {userIdentificacion} a Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}. La CUOTA a sido PAGADA COMPLETAMENTE (CANCELADA).";

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

                            Descripcion = $"El Usuario {userIdentificacion} a Realizado un PAGO DE CUOTA el dia {DateTime.Now}, cuyo valor es de ${Valor}, dejando un valor residual de ${cuotOriginal.Valor - Valor}. La CUOTA sigue estando PENDIENTE. ";


                        }
                        _context.Update(actCuota);
                        await _context.SaveChangesAsync();
                        await _nservices.CrearNotificacion( 3, userId, cuotOriginal.IdCuot, "PAGO DE CUOTA", Descripcion, "ADMINISTRADOR", new ActNotificacione());
                        CuotaRepository cobj = new CuotaRepository();
                        await _cpservices.SubirCapturaDePantalla( userId, "act_Cuotas", Id, CapturaPantalla, new ActCapturasPantalla());
                        CapturaPantallaRepository capobj = new CapturaPantallaRepository();
                        await _cpservices.ActualizarIdCapturaPantallaUser(Id, capobj.H_GetDataCapturaPantallaLastIdUser(userId), new ActCuota());
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
                        string Descripcion = $"El usuario {userIdentificacion} con C.I. {userCI} esta solicitando un prestamo de $ {actPrestamo.Valor} USD," +
                                                $"con fecha de entrega para el dia {actPrestamo.FechaEntregaDinero}, e inicio de pago de la deuda para el dia {actPrestamo.FechaInicioPagoCuotas}\n" +
                                                $"Estado: {actPrestamo.Estado}\n" + $"Tipo de Cuota: {actPrestamo.TipoCuota}";
                        PrestamosRepository pobj = new PrestamosRepository();
                        await _nservices.CrearNotificacion(4, IdUser, pobj.H_GetLastIdPres(IdUser), "PETICION DE PRESTAMO", Descripcion, "ADMINISTRADOR", new ActNotificacione());

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
        public async Task<IActionResult> PagoMulta([Bind("Id,IdMult,IdUser,FechaGeneracion,Cuadrante,Razon,Valor,Estado")]ActMulta actMulta)
        {

            return View(actMulta);
        }
    }
}
