using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using act_Application.Models;
using act_Application.Logica;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using System.Security.Cryptography;

namespace act_Application.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string Correo, string Contrasena)
        {
            string hashedPassword = HashPassword(Contrasena);

            ActUser objeto = new MetodoLogeo().EncontrarUser(Correo, hashedPassword);
            var claims = new List<Claim>();

            if (objeto.NombreYapellido != null)
            {
                // Obtener la información del rol usando el método DatosRolesUser
                MetodoLogeo metodoLogeo = new MetodoLogeo();
                int IdRol = metodoLogeo.ObtenerIdRolUsuario(objeto.Id);

                ActRol objetoRol = metodoLogeo.DatosRolesUser(IdRol);

                if (objetoRol != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, objeto.NombreYapellido));
                    claims.Add(new Claim(ClaimTypes.Email, objeto.Correo));
                    claims.Add(new Claim("Cedula", objeto.Cedula.ToString()));
                    claims.Add(new Claim("Rol", objetoRol.NombreRol));
                    claims.Add(new Claim("DescripcionRol", objetoRol.DescripcionRol));
                    claims.Add(new Claim("IdRol", IdRol.ToString()));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    //agregar componentes para después, como por ejemplo caducidad
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15)
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            // Aquí puedes seguir utilizando la variable "claims" si es necesario

            return RedirectToAction("Menu", "Home");
        }
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
