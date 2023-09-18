using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using act_Application.Logica;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using System.Security.Cryptography;
using act_Application.Models.BD;

namespace act_Application.Controllers.General
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

            if (!ModelState.IsValid) // Verificacion desde la Tabla Modelo
            {
                return View(); // Confirmacion de Error
            }
            if (!new MetodoLogeo().ValidarCorreo(Correo)) //Validacion desde la incersion de datos
            {
                ViewBag.ErrorMessage = "Caracteres especiales o formato de correo incorrecto detectado, corríjalo por favor.";
                ViewBag.ShowErrorMessage = true;
                return View();
            }

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
                    claims.Add(new Claim("Identificacion", objeto.NombreYapellido));
                    claims.Add(new Claim("CI", objeto.Cedula));
                    claims.Add(new Claim("Id", objeto.Id.ToString()));
                    claims.Add(new Claim("Rol", objetoRol.NombreRol));
                    claims.Add(new Claim("IdRol", IdRol.ToString()));
                    claims.Add(new Claim("TipoUsuario", objeto.TipoUser));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(16)
                };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                try
                {
                    new MetodoLogeo().EnviarNotificacionInicioSesion(objeto);
                }
                catch(Exception ex)
                {
                    Thread.Sleep(5000);
                    Console.WriteLine("Hubo un problema al enviar la notificación por correo electrónico.");
                    Console.WriteLine("Detalles del error: " + ex.Message);
                }
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Login", new { error = "Contraseña incorrecta" });
            }

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
