using act_Application.Data.Context;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Services
{
    public class CapturaDePantallaServices
    {
        private readonly ActDesarrolloContext _context;
        public CapturaDePantallaServices(ActDesarrolloContext context)
        {
            _context = context;
        }
        public async Task SubirCapturaDePantalla( int IdUser, string Origen, int IdOrigenCaptura, [FromForm] IFormFile CapturaPantalla, [Bind("Id,IdUser,Origen,IdOrigen,CapturaPantalla")] ActCapturasPantalla actCapturasPantalla)
        {
            actCapturasPantalla.IdUser = IdUser;
            actCapturasPantalla.Origen = Origen;
            actCapturasPantalla.IdOrigenCaptura = IdOrigenCaptura;
            if (CapturaPantalla != null && CapturaPantalla.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await CapturaPantalla.CopyToAsync(ms);
                    var bytes = ms.ToArray();
                    actCapturasPantalla.CapturaPantalla = bytes; // Asigna los bytes de la imagen a la propiedad CapturaPantalla
                }
            }
            _context.Add(actCapturasPantalla);
            await _context.SaveChangesAsync();
        }
    }
}
