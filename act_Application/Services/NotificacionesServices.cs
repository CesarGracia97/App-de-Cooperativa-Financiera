using act_Application.Data.Context;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Services
{
    public class NotificacionesServices
    {
        private readonly ActDesarrolloContext _context;
        public NotificacionesServices(ActDesarrolloContext context)
        {
            _context = context;
        }
        public async Task CrearNotificacion(int opcion, int IdUser, string IdActividad, string Razon, string Descripcion, string Destino, [Bind("Id,IdActividad,FechaGeneracion,Razon,Descripcion,Destino,Visto")] ActNotificacione actNotificacione)
        {
            switch (opcion)
            {
                case 1:
                    //Si es Nuevo Usuario.
                    actNotificacione.IdUser = IdUser;
                    actNotificacione.IdActividad = IdActividad;
                    actNotificacione.FechaGeneracion = DateTime.Now;
                    actNotificacione.Razon = $"| NUSER-{IdActividad} | {Razon}";
                    actNotificacione.Descripcion = Descripcion;
                    actNotificacione.Destino = Destino;
                    actNotificacione.Visto = "";
                    _context.Add(actNotificacione);
                    await _context.SaveChangesAsync();
                    break;
                case 2:
                    //Si es una Transaccion de Aportacion
                    actNotificacione.IdUser = IdUser;
                    actNotificacione.IdActividad = IdActividad;
                    actNotificacione.FechaGeneracion = DateTime.Now;
                    actNotificacione.Razon = $"| {IdActividad} | {Razon}";
                    actNotificacione.Descripcion = Descripcion;
                    actNotificacione.Destino = Destino;
                    actNotificacione.Visto = "";
                    _context.Add(actNotificacione);
                    await _context.SaveChangesAsync();
                    break;
                case 3:
                    //Si es una Transaccion de Pago de Cuotas
                    actNotificacione.IdUser = IdUser;
                    actNotificacione.IdActividad = IdActividad;
                    actNotificacione.FechaGeneracion = DateTime.Now;
                    actNotificacione.Razon = $"| {IdActividad} | {Razon}";
                    actNotificacione.Descripcion = Descripcion;
                    actNotificacione.Destino = Destino;
                    actNotificacione.Visto = "";
                    _context.Add(actNotificacione);
                    await _context.SaveChangesAsync();
                    break;
                case 4:
                    //Si es una Transaccion de Prestamo
                    actNotificacione.IdUser = IdUser;
                    actNotificacione.IdActividad = IdActividad;
                    actNotificacione.FechaGeneracion = DateTime.Now;
                    actNotificacione.Razon = $"| {IdActividad} | {Razon}";
                    actNotificacione.Descripcion = Descripcion;
                    actNotificacione.Destino = Destino;
                    actNotificacione.Visto = "";
                    _context.Add(actNotificacione);
                    await _context.SaveChangesAsync();
                    break;
                case 5:
                    //Si es una Transaccion de Pago de Multas
                    actNotificacione.IdUser = IdUser;
                    actNotificacione.IdActividad = IdActividad;
                    actNotificacione.FechaGeneracion = DateTime.Now;
                    actNotificacione.Razon = $"| {IdActividad} | {Razon}";
                    actNotificacione.Descripcion = Descripcion;
                    actNotificacione.Destino = Destino;
                    actNotificacione.Visto = "";
                    _context.Add(actNotificacione);
                    await _context.SaveChangesAsync();
                    break;
                case 6:
                    //Si es una participacion de Evento de Prestamo
                    actNotificacione.IdUser = IdUser;
                    actNotificacione.IdActividad = IdActividad;
                    actNotificacione.FechaGeneracion = DateTime.Now;
                    actNotificacione.Razon = $"| {IdActividad} | {Razon}";
                    actNotificacione.Descripcion = Descripcion;
                    actNotificacione.Destino = Destino;
                    actNotificacione.Visto = "";
                    _context.Add(actNotificacione);
                    await _context.SaveChangesAsync();
                    break;
                default:
                    break;
            }
        }
    }
}
