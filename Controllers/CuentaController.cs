using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaStock.Models;
using Microsoft.AspNetCore.Http;

namespace SistemaStock.Controllers
{
    public class CuentaController : Controller
    {
        private readonly SistemaStockContext _context;

        public CuentaController(SistemaStockContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == model.NombreUsuario
                                           && u.Password == model.Password
                                           && u.Activo == true);

                if (usuario != null)
                {
                    // Guardar usuario en sesión
                    HttpContext.Session.SetString("UsuarioId", usuario.UsuarioId.ToString());
                    HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
                    HttpContext.Session.SetString("Nombre", usuario.Nombre);
                    HttpContext.Session.SetString("RolId", usuario.RolId.ToString());

                    return RedirectToAction("Index", "Productoes");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                }
            }
            return View(model);
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el usuario ya existe
                var usuarioExiste = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == model.NombreUsuario);

                if (usuarioExiste)
                {
                    ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya existe");
                    return View(model);
                }

                // Crear nuevo usuario
                var nuevoUsuario = new Usuarios
                {
                    NombreUsuario = model.NombreUsuario,
                    Password = model.Password, // En producción deberías encriptar esto
                    Nombre = model.Nombre,
                    RolId = 2, // Rol de Usuario normal
                    Activo = true
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
