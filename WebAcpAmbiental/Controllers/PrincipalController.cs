using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class PrincipalController : Controller
    {


        private readonly AcpAmbientalContext _context;

        public PrincipalController(AcpAmbientalContext context)
        {
            _context = context;
        }



        public IActionResult Principal()
        {
            // Obtener el nombre de usuario y rol de la sesión
            var usuario = HttpContext.Session.GetString("Usuario");
            var rol = HttpContext.Session.GetString("Rol");

            // Verificar si se encontró un usuario y rol en la sesión
            if (usuario != null && rol != null)
            {
                ViewBag.Usuario = usuario;
                ViewBag.Rol = rol; // Asegúrate de que el rol está siendo pasado correctamente
            }
            else
            {
                // Si no hay usuario o rol en la sesión, redirigir al inicio
                return RedirectToAction("Index", "Home");
            }

            // Pasar mensaje desde TempData a ViewBag
            if (TempData["SweetAlertMessage"] != null)
            {
                ViewBag.SweetAlertMessage = TempData["SweetAlertMessage"].ToString();
            }

            // Consultas a la base de datos
            var cantidadClientes = _context.MaestroCliente.Count();
            var cantidadVendedores = _context.Usuario.Count();
            var cantidadClientesParticulares = _context.MaestroCliente.Where(c => c.TipoCliente == "particular").Count();
            var cantidadClientesJuridicos = _context.MaestroCliente.Where(c => c.TipoCliente == "juridico").Count();

            // Obtener la asignación de canal, zona y ruta del usuario (suponiendo que el usuario tiene una asignación)
            var asignacionUsuario = _context.Asignacion
                .Where(a => a.IdUsuarioNavigation.UserUsuario == usuario)
                .Include(a => a.IdCanalNavigation)
                .Include(a => a.IdZonaNavigation)
                .Include(a => a.IdRutaNavigation)
                .FirstOrDefault();

            if (asignacionUsuario != null)
            {
                ViewBag.Canal = asignacionUsuario.IdCanalNavigation.Descripcion;
                ViewBag.Zona = asignacionUsuario.IdZonaNavigation.Descripcion;
                ViewBag.Ruta = asignacionUsuario.IdRutaNavigation.Descripcion;
            }
            else
            {
                ViewBag.Canal = "No asignado";
                ViewBag.Zona = "No asignado";
                ViewBag.Ruta = "No asignado";
            }

            // Pasar los datos a la vista
            ViewBag.CantidadClientes = cantidadClientes;
            ViewBag.CantidadVendedores = cantidadVendedores;
            ViewBag.CantidadClientesParticulares = cantidadClientesParticulares;
            ViewBag.CantidadClientesJuridicos = cantidadClientesJuridicos;

            return View(); // Carga la vista /Views/Principal/Principal.cshtml
        }


        [HttpGet]
        public IActionResult GetBarChartData()
        {
            var data = new
            {
                labels = new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo" },
                values = new[] { 1500, 2000, 1800, 2200, 2100 }
            };
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetDonutChartData()
        {
            var data = new
            {
                labels = new[] { "Productos A", "Productos B", "Productos C" },
                values = new[] { 300, 500, 200 }
            };
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetLineChartData()
        {
            var data = new
            {
                labels = new[] { "2020", "2021", "2022", "2023", "2024" },
                values = new[] { 100, 200, 300, 400, 500 }
            };
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetPieChartData()
        {
            var data = new
            {
                labels = new[] { "Categoría X", "Categoría Y", "Categoría Z" },
                values = new[] { 50, 30, 20 }
            };
            return Json(data);
        }

    }
}
