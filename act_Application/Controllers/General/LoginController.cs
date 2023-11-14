using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using act_Application.Logic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using System.Security.Cryptography;
using act_Application.Models.BD;
using act_Application.Data.Data;
using act_Application.Models.Sistema.Complementos;
using act_Application.Models.Sistema.ViewModel;
using act_Application.Data.Context;
using act_Application.Services;

namespace act_Application.Controllers.General
{
    public class LoginController : Controller
    {
        private readonly ActDesarrolloContext _context;
        public LoginController(ActDesarrolloContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string Correo, string Contrasena)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!new MetodoLogeo().ValidarCorreo(Correo))
            {
                ViewBag.ErrorMessage = "Caracteres especiales o formato de correo incorrecto detectado, corríjalo por favor.";
                ViewBag.ShowErrorMessage = true;
                return View();
            }

            string hashedPassword = HashPassword(Contrasena);

            ActUser objeto = new UsuarioRepository().GetDataUser(Correo, hashedPassword);
            var claims = new List<Claim>();

            if (objeto != null)
            {
                if (objeto.Estado != "ACTIVO")
                {
                    string mensaje = string.Empty;

                    // Establecer el mensaje según el estado del usuario
                    switch (objeto.Estado)
                    {
                        case "INACTIVO":
                            return RedirectToAction("Cuenta_Inactiva", "Login");
                        case "DENEGADO":
                            return RedirectToAction("Cuenta_Denegada", "Login");
                        case "EN EVALUACION":
                            return RedirectToAction("Cuenta_en_Evaluacion", "Login");
                        default:
                            return RedirectToAction("CSE", "Login");
                    }
                }
                if (objeto.NombreYapellido != null)
                {
                    UsuarioRepository usuarioRepository = new UsuarioRepository();

                    ActRol objetoRol = usuarioRepository.GetDataRolUser(objeto.IdRol);

                    if (objetoRol != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Name, objeto.NombreYapellido));
                        claims.Add(new Claim(ClaimTypes.Email, objeto.Correo));
                        claims.Add(new Claim("CI", objeto.Cedula));
                        claims.Add(new Claim("Id", objeto.Id.ToString()));
                        claims.Add(new Claim("IdRol", objeto.IdRol.ToString()));
                        claims.Add(new Claim("Rol", objetoRol.NombreRol));
                        claims.Add(new Claim("TipoUsuario", objeto.TipoUser));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    try
                    {
                        new MetodoLogeo().EnviarNotificacionInicioSesion(objeto);
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("Hubo un problema al enviar la notificación por correo electrónico.");
                        Console.WriteLine("Detalles del error: " + ex.Message);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Contraseña incorrecta";
            }

            return RedirectToAction("Index", "Login");
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
        public IActionResult Registrarse()
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            List<UserList> listUsuarios = usuarioRepository.GetDataListUser();

            var itemListUsuarios = listUsuarios.Select(usuarios =>
            new
            {
                Value = $"{usuarios.Id}",
                Text = $"{usuarios.Usuario}"
            }).ToList();

            var viewModel = new Registro_VM
            {
                ListaDeUsuarios = listUsuarios
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(string NombreYapellido, string Cedula, string Correo, string Contrasena, int IdSocio, string Celular, [FromForm] IFormFile FotoPerfil, [Bind("Id,Cedula,Correo,NombreYApellido,Celular,Contrasena,TipoUser,IdSocio,FotoPerfil,Estado")] ActUser actUser)
        {
            if (ModelState.IsValid)
            {
                string Razon = "Nuevo Usuario Registrado - Peticion de Adminision";
                string Descripcion = string.Empty;
                string Destino = "TODOS";
                actUser.Cedula = Cedula;
                actUser.NombreYapellido = NombreYapellido;
                actUser.Correo = Correo;
                actUser.Contrasena = HashPassword(Contrasena);
                actUser.IdSocio = IdSocio;
                actUser.Celular = Celular;

                if (FotoPerfil != null && FotoPerfil.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await FotoPerfil.CopyToAsync(ms);
                        var bytes = ms.ToArray();
                        actUser.FotoPerfil = bytes;
                    }
                }
                actUser.TipoUser = "En Espera";         //4 ESTADOS =  Administrador, Socio, Referido, En Espera
                actUser.Estado = "EN EVALUACION";       //4 ESTADO = ACTIVO, INACTIVO, EN EVALUACION, NEGADO.
                _context.Add(actUser);
                Descripcion = $"La Persona de nombre {actUser.NombreYapellido} y C.I. {actUser.Cedula}, con Id de Referente {actUser.IdSocio} ha mandado una peticion de Adminicion a la Organizacion";
                NotificacionesServices notificacion = new NotificacionesServices(_context);
                await notificacion.CrearNotificacion( 1, 0, actUser.Cedula, Razon, Descripcion, Destino, new ActNotificacione());

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Login");
            }
            return View(actUser);
        }
        public IActionResult Cuenta_en_Evaluacion()
        {
            return View();
        }
        public IActionResult Cuenta_Denegada()
        {
            return View();
        }
        public IActionResult Cuenta_Inactiva()
        {
            return View();
        }
        public IActionResult CSE()
        {
            return View();
        }
    }
}
