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
        public async Task CrearNotificacion(int opcion, string IdActividad, string Razon, string Descripcion, string Destino, [Bind("Id,IdActividad,FechaGeneracion,Razon,Descripcion,Destino,Visto")] ActNotificacione actNotificacione)
        {
            switch (opcion)
            {
                case 1:
                    //Si es Nuevo Usuario.
                    actNotificacione.IdUser = 0; // ES 0 AL TRATARSE DE ALGUIEN QUE NO SE ENCUENTRA EN EL SISTEMA.
                    actNotificacione.IdActividad = IdActividad;
                    actNotificacione.FechaGeneracion = DateTime.Now;
                    actNotificacione.Razon = Razon;
                    actNotificacione.Descripcion = Descripcion;
                    actNotificacione.Destino = Destino;
                    actNotificacione.Visto = "";

                    _context.Add(actNotificacione);
                    await _context.SaveChangesAsync();

                    break;
                case 2:
                    //Si es una Transaccion de Aportacion

                    break;
                case 3:
                    //Si es una Transaccion de Pago de Cuotas

                    break;
                default:
                    break;
            }
        }
    }
}
