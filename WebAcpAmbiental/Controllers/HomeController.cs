using Microsoft.AspNetCore.Mvc;
using WebAcpAmbiental.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // Asegúrate de importar el namespace para el logger

namespace WebAcpAmbiental.Controllers
{
    public class HomeController : Controller
    {
        private readonly AcpAmbientalContext _context;
        private readonly ILogger<HomeController> _logger;  // Inyección del logger

        // Inyección del contexto y el logger
        public HomeController(AcpAmbientalContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;  // Asignación del logger
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Principal(string username, string password)
        {
            try
            {
                // Lógica del controlador
                var usuario = _context.Usuario
                    .Include(u => u.IdRolNavigation)
                    .FirstOrDefault(u => u.UserUsuario == username && u.PasswordUsuario == password);

                if (usuario != null)
                {
                    if (usuario.IdEstadoNavigation != null && usuario.IdEstadoNavigation.Descripcion == "Inactivo")
                    {
                        ViewBag.Error = "El usuario está inactivo. Contacta al administrador.";
                        return View("Index");
                    }

                    var rol = usuario.IdRolNavigation?.Descripcion ?? "Sin rol";

                    HttpContext.Session.SetString("Usuario", usuario.UserUsuario);
                    HttpContext.Session.SetString("Rol", rol);

                    TempData["SweetAlertMessage"] = $"Datos correctos, Bienvenido al sistema, {usuario.UserUsuario}";
                    return RedirectToAction("Principal", "Principal");
                }
                else
                {
                    ViewBag.Error = "Usuario o contraseña incorrectos.";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                // Registrar el error en el log
                _logger.LogError(ex, "Error al procesar la solicitud en la acción Principal.");

                // Mostrar el error al usuario
                ViewBag.Error = $"Ocurrió un error inesperado: {ex.Message}";
                return View("Index");
            }
        }
    }
}
