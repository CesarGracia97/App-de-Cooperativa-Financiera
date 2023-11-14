using act_Application.Data.Context;

namespace act_Application.Services
{
    public class CapturaDePantallaServices
    {
        private readonly ActDesarrolloContext _context;
        public CapturaDePantallaServices(ActDesarrolloContext context)
        {
            _context = context;
        }
    }
}
