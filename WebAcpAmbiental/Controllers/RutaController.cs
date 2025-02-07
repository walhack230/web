using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class RutaController : Controller
    {

        private readonly AcpAmbientalContext _context;

        public RutaController(AcpAmbientalContext context)
        {
            _context = context;
        }


        public IActionResult Ruta()
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
            var rutas = _context.Ruta
                .Include(u => u.IdEstadoNavigation)
                .ToList();

            // Pasar la lista de usuarios a la vista
            return View(rutas);
            //return View("~/Views/Usuario/Usuario.cshtml"); 

        }


        [HttpPost]
        public IActionResult Create([FromBody] Ruta rutas)
        {
            try
            {
                _context.Ruta.Add(rutas);
                _context.SaveChanges();
                return Json(new { success = true, ruta = rutas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        [Route("Ruta/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                // Buscar el usuario en la base de datos
                var ruta = _context.Ruta.FirstOrDefault(u => u.IdRuta == id);
                if (ruta == null)
                {
                    return Json(new { success = false, message = "Ruta no encontrada." });
                }

                // Eliminar el usuario
                _context.Ruta.Remove(ruta);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerRuta(int id)
        {
            var ruta = _context.Ruta
                .Where(r => r.IdRuta == id)
                .Select(r => new
                {
                    r.IdRuta,
                    r.Descripcion,
                    
                    r.IdEstado,
                    
                    Estado = r.IdEstadoNavigation != null ? r.IdEstadoNavigation.Descripcion : null,
                    
                })
                .FirstOrDefault();

            if (ruta == null)
            {
                return NotFound(new { mensaje = "Ruta no encontrada" });
            }

            return Json(ruta);
        }


        [HttpPost]
        public IActionResult ActualizarRuta([FromBody] Ruta ruta)
        {
            try
            {
                if (ruta == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío" });
                }

                var rutaExistente = _context.Ruta
                    .Where(r => r.IdRuta == ruta.IdRuta)
                    .FirstOrDefault();

                if (rutaExistente == null)
                {
                    return NotFound(new { message = "Ruta no encontrada" });
                }

                // Actualizar los datos del usuario
                rutaExistente.Descripcion = ruta.Descripcion;
                
                rutaExistente.IdEstado = ruta.IdEstado;
               

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                return Ok(new { message = "Ruta actualizada correctamente" });
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
