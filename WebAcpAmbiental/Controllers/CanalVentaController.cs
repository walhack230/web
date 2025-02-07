using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class CanalVentaController : Controller
    {



        private readonly AcpAmbientalContext _context;

        public CanalVentaController(AcpAmbientalContext context)
        {
            _context = context;
        }

        public IActionResult CanalVenta()
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

            // Recuperar los usuarios y sus relaciones
            var canalventa = _context.CanalVenta
                .Include(u => u.IdEstadoNavigation)
                .ToList();

            // Pasar la lista de usuarios a la vista
            return View(canalventa);
            //return View("~/Views/Usuario/Usuario.cshtml"); 

        }


        [HttpPost]
        public IActionResult Create([FromBody] CanalVenta canalventas)
        {
            try
            {
                _context.CanalVenta.Add(canalventas);
                _context.SaveChanges();
                return Json(new { success = true, canalventa = canalventas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("CanalVenta/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                // Buscar el usuario en la base de datos
                var canalventas = _context.CanalVenta.FirstOrDefault(u => u.IdCanal == id);
                if (canalventas == null)
                {
                    return Json(new { success = false, message = "Canal no encontrada." });
                }

                // Eliminar el usuario
                _context.CanalVenta.Remove(canalventas);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerCanalVenta(int id)
        {
            var canalventas = _context.CanalVenta
                .Where(c => c.IdCanal == id)
                .Select(c => new
                {
                    c.IdCanal,
                    c.Descripcion,

                    c.IdEstado,

                    Estado = c.IdEstadoNavigation != null ? c.IdEstadoNavigation.Descripcion : null,

                })
                .FirstOrDefault();

            if (canalventas == null)
            {
                return NotFound(new { mensaje = "Ruta no encontrada" });
            }

            return Json(canalventas);
        }

        [HttpPost]
        public IActionResult ActualizarCanalVenta([FromBody] CanalVenta canalventas)
        {
            try
            {
                if (canalventas == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío" });
                }

                var canalExistente = _context.CanalVenta
                    .Where(r => r.IdCanal == canalventas.IdCanal)
                    .FirstOrDefault();

                if (canalExistente == null)
                {
                    return NotFound(new { message = "Canal de venta no encontrada" });
                }

                // Actualizar los datos del usuario
                canalExistente.Descripcion = canalventas.Descripcion;

                canalExistente.IdEstado = canalventas.IdEstado;


                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                return Ok(new { message = "Canal actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error en el servidor", error = ex.Message });
            }
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



            // Consultas a la base de datos
            var cantidadClientes = _context.MaestroCliente.Count();
            var cantidadVendedores = _context.Usuario.Count();
            var cantidadClientesParticulares = _context.MaestroCliente.Where(c => c.TipoCliente == "Particular").Count();
            var cantidadClientesJuridicos = _context.MaestroCliente.Where(c => c.TipoCliente == "Jurídico").Count();



            // Pasar los datos a la vista
            ViewBag.CantidadClientes = cantidadClientes;
            ViewBag.CantidadVendedores = cantidadVendedores;
            ViewBag.CantidadClientesParticulares = cantidadClientesParticulares;
            ViewBag.CantidadClientesJuridicos = cantidadClientesJuridicos;

            return View("~/Views/Principal/Principal.cshtml");



        }
    }
}
