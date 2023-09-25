using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using act_Application.Data.Data;
using act_Application.Models.BD;

namespace act_Application.Controllers.Administrador.BD
{
    public class AdministradorController : Controller
    {

        private readonly DesarrolloContext _context;

        public AdministradorController(DesarrolloContext context)
        {
            _context = context;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------Aportaciones
        public async Task<IActionResult> AdminAportaciones()
        {
            return View(await _context.ActAportaciones.ToListAsync());
        }

        public IActionResult CreateAportaciones()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAportaciones([Bind("Id,Razon,Valor,IdUser,FechaAportacion,Aprobacion,Cuadrante1,Cuadrante2,Nbanco,Cbancaria,CapturaPantalla,BancoDestino,CuentaDestino")] ActAportacione actAportacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actAportacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actAportacione);
        }

        public async Task<IActionResult> EditAportaciones(int? id)
        {
            if (id == null || _context.ActAportaciones == null)
            {
                return NotFound();
            }

            var actAportacione = await _context.ActAportaciones.FindAsync(id);
            if (actAportacione == null)
            {
                return NotFound();
            }
            return View(actAportacione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAportaciones(int id, [Bind("Id,Razon,Valor,IdUser,FechaAportacion,Aprobacion,Cuadrante1,Cuadrante2,Nbanco,Cbancaria,CapturaPantalla,BancoDestino,CuentaDestino")] ActAportacione actAportacione)
        {
            if (id != actAportacione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actAportacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActAportacioneExists(actAportacione.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actAportacione);
        }
        public async Task<IActionResult> DeleteAportaciones(int? id)
        {
            if (id == null || _context.ActAportaciones == null)
            {
                return NotFound();
            }

            var actAportacione = await _context.ActAportaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actAportacione == null)
            {
                return NotFound();
            }

            return View(actAportacione);
        }

        [HttpPost, ActionName("DeleteAportaciones")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAportaciones(int id)
        {
            if (_context.ActAportaciones == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActAportaciones'  is null.");
            }
            var actAportacione = await _context.ActAportaciones.FindAsync(id);
            if (actAportacione != null)
            {
                _context.ActAportaciones.Remove(actAportacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActAportacioneExists(int id)
        {
            return _context.ActAportaciones.Any(e => e.Id == id);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------Cuotas

        public async Task<IActionResult> AdminCuotas()
        {
            return View(await _context.ActCuotas.ToListAsync());
        }

        public IActionResult CreateCuotas()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCuotas([Bind("Id,ValorCuota,FechaCuota,Estado,IdUser,IdTransaccion")] ActCuota actCuota)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actCuota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actCuota);
        }

        public async Task<IActionResult> EditCuotas(int? id)
        {
            if (id == null || _context.ActCuotas == null)
            {
                return NotFound();
            }

            var actCuota = await _context.ActCuotas.FindAsync(id);
            if (actCuota == null)
            {
                return NotFound();
            }
            return View(actCuota);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCuotas(int id, [Bind("Id,ValorCuota,FechaCuota,Estado,IdUser,IdTransaccion")] ActCuota actCuota)
        {
            if (id != actCuota.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actCuota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActCuotaExists(actCuota.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actCuota);
        }

        public async Task<IActionResult> DeleteCuotas(int? id)
        {
            if (id == null || _context.ActCuotas == null)
            {
                return NotFound();
            }

            var actCuota = await _context.ActCuotas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actCuota == null)
            {
                return NotFound();
            }

            return View(actCuota);
        }

        [HttpPost, ActionName("DeleteCuotas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedCuotas(int id)
        {
            if (_context.ActCuotas == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActCuotas'  is null.");
            }
            var actCuota = await _context.ActCuotas.FindAsync(id);
            if (actCuota != null)
            {
                _context.ActCuotas.Remove(actCuota);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActCuotaExists(int id)
        {
            return _context.ActCuotas.Any(e => e.Id == id);
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------Multas

        public async Task<IActionResult> AdminMultas()
        {
            return View(await _context.ActMultas.ToListAsync());
        }

        public IActionResult CreateMultas()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMultas([Bind("Id,IdUser,Porcentaje,Valor,FechaMulta,IdAportacion,Cuadrante1,Cuadrante2")] ActMulta actMulta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actMulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actMulta);
        }

        public async Task<IActionResult> EditMultas(int? id)
        {
            if (id == null || _context.ActMultas == null)
            {
                return NotFound();
            }

            var actMulta = await _context.ActMultas.FindAsync(id);
            if (actMulta == null)
            {
                return NotFound();
            }
            return View(actMulta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMultas(int id, [Bind("Id,IdUser,Porcentaje,Valor,FechaMulta,IdAportacion,Cuadrante1,Cuadrante2")] ActMulta actMulta)
        {
            if (id != actMulta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actMulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActMultaExists(actMulta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actMulta);
        }

        public async Task<IActionResult> DeleteMultas(int? id)
        {
            if (id == null || _context.ActMultas == null)
            {
                return NotFound();
            }

            var actMulta = await _context.ActMultas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actMulta == null)
            {
                return NotFound();
            }

            return View(actMulta);
        }

        [HttpPost, ActionName("DeleteMultas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedMultas(int id)
        {
            if (_context.ActMultas == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActMultas'  is null.");
            }
            var actMulta = await _context.ActMultas.FindAsync(id);
            if (actMulta != null)
            {
                _context.ActMultas.Remove(actMulta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActMultaExists(int id)
        {
            return _context.ActMultas.Any(e => e.Id == id);
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------Notificaciones

        public async Task<IActionResult> AdminNotificaciones()
        {
            return View(await _context.ActNotificaciones.ToListAsync());
        }

        public IActionResult CreateNotificaciones()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNotificaciones([Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino,IdTransacciones,IdAportaciones,IdCuotas,Visto")] ActNotificacione actNotificacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actNotificacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actNotificacione);
        }

        public async Task<IActionResult> EditNotificaciones(int? id)
        {
            if (id == null || _context.ActNotificaciones == null)
            {
                return NotFound();
            }

            var actNotificacione = await _context.ActNotificaciones.FindAsync(id);
            if (actNotificacione == null)
            {
                return NotFound();
            }
            return View(actNotificacione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNotificaciones(int id, [Bind("Id,IdUser,Razon,Descripcion,FechaNotificacion,Destino,IdTransacciones,IdAportaciones,IdCuotas,Visto")] ActNotificacione actNotificacione)
        {
            if (id != actNotificacione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actNotificacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActNotificacioneExists(actNotificacione.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actNotificacione);
        }

        public async Task<IActionResult> DeleteNotificaciones(int? id)
        {
            if (id == null || _context.ActNotificaciones == null)
            {
                return NotFound();
            }

            var actNotificacione = await _context.ActNotificaciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actNotificacione == null)
            {
                return NotFound();
            }

            return View(actNotificacione);
        }

        [HttpPost, ActionName("DeleteNotificaciones")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedNotificaciones(int id)
        {
            if (_context.ActNotificaciones == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActNotificaciones'  is null.");
            }
            var actNotificacione = await _context.ActNotificaciones.FindAsync(id);
            if (actNotificacione != null)
            {
                _context.ActNotificaciones.Remove(actNotificacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActNotificacioneExists(int id)
        {
            return _context.ActNotificaciones.Any(e => e.Id == id);
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------ParticipantesId

        public async Task<IActionResult> AdminParticipantes()
        {
            return View(await _context.ActParticipantes.ToListAsync());
        }

        // GET: ActParticipantes/Create
        public IActionResult CreateParticipantes()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateParticipantes([Bind("Id,IdTransaccion,FechaInicio,FechaFinalizacion,FechaGeneracion,ParticipantesId,Estado")] ActParticipante actParticipante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actParticipante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actParticipante);
        }

        public async Task<IActionResult> EditParticipantes(int? id)
        {
            if (id == null || _context.ActParticipantes == null)
            {
                return NotFound();
            }

            var actParticipante = await _context.ActParticipantes.FindAsync(id);
            if (actParticipante == null)
            {
                return NotFound();
            }
            return View(actParticipante);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditParticipantes(int id, [Bind("Id,IdTransaccion,FechaInicio,FechaFinalizacion,FechaGeneracion,ParticipantesId,Estado")] ActParticipante actParticipante)
        {
            if (id != actParticipante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actParticipante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActParticipanteExists(actParticipante.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actParticipante);
        }

        public async Task<IActionResult> DeleteParticipantes(int? id)
        {
            if (id == null || _context.ActParticipantes == null)
            {
                return NotFound();
            }

            var actParticipante = await _context.ActParticipantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actParticipante == null)
            {
                return NotFound();
            }

            return View(actParticipante);
        }

        [HttpPost, ActionName("DeleteParticipantes")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedParticipantes(int id)
        {
            if (_context.ActParticipantes == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActParticipantes'  is null.");
            }
            var actParticipante = await _context.ActParticipantes.FindAsync(id);
            if (actParticipante != null)
            {
                _context.ActParticipantes.Remove(actParticipante);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActParticipanteExists(int id)
        {
            return _context.ActParticipantes.Any(e => e.Id == id);
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------Roles

        public async Task<IActionResult> AdminRoles()
        {
            return View(await _context.ActRols.ToListAsync());
        }

        public IActionResult CreateRoles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoles([Bind("Id,NombreRol,DescripcionRol")] ActRol actRol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actRol);
        }

        public async Task<IActionResult> EditRoles(int? id)
        {
            if (id == null || _context.ActRols == null)
            {
                return NotFound();
            }

            var actRol = await _context.ActRols.FindAsync(id);
            if (actRol == null)
            {
                return NotFound();
            }
            return View(actRol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(int id, [Bind("Id,NombreRol,DescripcionRol")] ActRol actRol)
        {
            if (id != actRol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actRol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActRolExists(actRol.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actRol);
        }

        public async Task<IActionResult> DeleteRoles(int? id)
        {
            if (id == null || _context.ActRols == null)
            {
                return NotFound();
            }

            var actRol = await _context.ActRols
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actRol == null)
            {
                return NotFound();
            }

            return View(actRol);
        }

        [HttpPost, ActionName("DeleteRoles")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedRoles(int id)
        {
            if (_context.ActRols == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActRols'  is null.");
            }
            var actRol = await _context.ActRols.FindAsync(id);
            if (actRol != null)
            {
                _context.ActRols.Remove(actRol);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActRolExists(int id)
        {
            return _context.ActRols.Any(e => e.Id == id);
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------Roles de Usuario

        public async Task<IActionResult> AdminRolesUser()
        {
            return View(await _context.ActRolUsers.ToListAsync());
        }

        public IActionResult CreateRolesUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRolesUser([Bind("Id,IdUser,IdRol")] ActRolUser actRolUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actRolUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actRolUser);
        }

        public async Task<IActionResult> EditRolesUser(int? id)
        {
            if (id == null || _context.ActRolUsers == null)
            {
                return NotFound();
            }

            var actRolUser = await _context.ActRolUsers.FindAsync(id);
            if (actRolUser == null)
            {
                return NotFound();
            }
            return View(actRolUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRolesUser(int id, [Bind("Id,IdUser,IdRol")] ActRolUser actRolUser)
        {
            if (id != actRolUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actRolUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActRolUserExists(actRolUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actRolUser);
        }

        public async Task<IActionResult> DeleteRolesUser(int? id)
        {
            if (id == null || _context.ActRolUsers == null)
            {
                return NotFound();
            }

            var actRolUser = await _context.ActRolUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actRolUser == null)
            {
                return NotFound();
            }

            return View(actRolUser);
        }

        [HttpPost, ActionName("DeleteRolesUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedRolesUser(int id)
        {
            if (_context.ActRolUsers == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActRolUsers'  is null.");
            }
            var actRolUser = await _context.ActRolUsers.FindAsync(id);
            if (actRolUser != null)
            {
                _context.ActRolUsers.Remove(actRolUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActRolUserExists(int id)
        {
            return _context.ActRolUsers.Any(e => e.Id == id);
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------Transacciones

        public async Task<IActionResult> AdminTransacciones()
        {
            return View(await _context.ActTransacciones.ToListAsync());
        }

        public IActionResult CreateTransacciones()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransacciones([Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,FechaGeneracion,TipoCuota,IdParticipantes")] ActTransaccione actTransaccione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actTransaccione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actTransaccione);
        }

        public async Task<IActionResult> EditTransacciones(int? id)
        {
            if (id == null || _context.ActTransacciones == null)
            {
                return NotFound();
            }

            var actTransaccione = await _context.ActTransacciones.FindAsync(id);
            if (actTransaccione == null)
            {
                return NotFound();
            }
            return View(actTransaccione);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransacciones(int id, [Bind("Id,Razon,IdUser,Valor,Estado,FechaEntregaDinero,FechaPagoTotalPrestamo,FechaIniCoutaPrestamo,FechaGeneracion,TipoCuota,IdParticipantes")] ActTransaccione actTransaccione)
        {
            if (id != actTransaccione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actTransaccione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActTransaccioneExists(actTransaccione.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actTransaccione);
        }

        public async Task<IActionResult> DeleteTransacciones(int? id)
        {
            if (id == null || _context.ActTransacciones == null)
            {
                return NotFound();
            }

            var actTransaccione = await _context.ActTransacciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actTransaccione == null)
            {
                return NotFound();
            }

            return View(actTransaccione);
        }

        [HttpPost, ActionName("DeleteTransacciones")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedTransacciones(int id)
        {
            if (_context.ActTransacciones == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActTransacciones'  is null.");
            }
            var actTransaccione = await _context.ActTransacciones.FindAsync(id);
            if (actTransaccione != null)
            {
                _context.ActTransacciones.Remove(actTransaccione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActTransaccioneExists(int id)
        {
            return _context.ActTransacciones.Any(e => e.Id == id);
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------Usuarios

        public async Task<IActionResult> AdminUsuarios()
        {
            return View(await _context.ActUsers.ToListAsync());
        }


        public IActionResult CreateUsuarios()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsuarios([Bind("Id,Cedula,Correo,NombreYapellido,Celular,Contrasena,TipoUser,IdSocio,FotoPerfil,Activo")] ActUser actUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actUser);
        }

        public async Task<IActionResult> EditUsuarios(int? id)
        {
            if (id == null || _context.ActUsers == null)
            {
                return NotFound();
            }

            var actUser = await _context.ActUsers.FindAsync(id);
            if (actUser == null)
            {
                return NotFound();
            }
            return View(actUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsuarios(int id, [Bind("Id,Cedula,Correo,NombreYapellido,Celular,Contrasena,TipoUser,IdSocio,FotoPerfil,Activo")] ActUser actUser)
        {
            if (id != actUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActUserExists(actUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actUser);
        }

        public async Task<IActionResult> DeleteUsuarios(int? id)
        {
            if (id == null || _context.ActUsers == null)
            {
                return NotFound();
            }

            var actUser = await _context.ActUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actUser == null)
            {
                return NotFound();
            }

            return View(actUser);
        }

        // POST: ActUsers/Delete/5
        [HttpPost, ActionName("DeleteUsuarios")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedUsuarios(int id)
        {
            if (_context.ActUsers == null)
            {
                return Problem("Entity set 'DesarrolloContext.ActUsers'  is null.");
            }
            var actUser = await _context.ActUsers.FindAsync(id);
            if (actUser != null)
            {
                _context.ActUsers.Remove(actUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActUserExists(int id)
        {
            return _context.ActUsers.Any(e => e.Id == id);
        }

    }
}
