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
        public async Task CrearNotificacion(int opcion, [Bind("Id,IdActividad,FechaGeneracion,Razon,Descripcion,Destino,Visto")] ActNotificacione actNotificacione)
        {
            switch (opcion)
            {
                case 1:
                    break;
                    //Si es Nuevo Usuario.
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }
    }
}
