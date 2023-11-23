using act_Application.Data.Context;
using act_Application.Models.BD;
using Microsoft.AspNetCore.Mvc;

namespace act_Application.Services.ServiciosAplicativos
{
    public class InteresesServices
    {
        private readonly ActDesarrolloContext _context;
        public async Task AddNewInteres(int IdUser, [Bind("")]ActTablaInteres tablaInteres)
        {

        }
    }
}
