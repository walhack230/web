using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class ZonaController : Controller
    {
        private readonly AcpAmbientalContext _context;

        public ZonaController(AcpAmbientalContext context)
        {
            _context = context;
        }


      //listar zona

        public IActionResult Zona()
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
            var zonas = _context.Zona
                .Include(u => u.IdEstadoNavigation)
                .ToList();

            // Pasar la lista de usuarios a la vista
            return View(zonas);
          
        }

        //Crear zona
        [HttpPost]
        public IActionResult Create([FromBody] Zona zonas)
        {
            try
            {
                _context.Zona.Add(zonas);
                _context.SaveChanges();
                return Json(new { success = true, zona = zonas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        //Eliminar Zona

        [HttpPost]
        [Route("Zona/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                // Buscar el usuario en la base de datos
                var zona = _context.Zona.FirstOrDefault(u => u.IdZona == id);
                if (zona == null)
                {
                    return Json(new { success = false, message = "Zona no encontrada." });
                }

                // Eliminar el usuario
                _context.Zona.Remove(zona);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //Obtener el ID

        [HttpGet]
        public IActionResult ObtenerZona(int id)
        {
            var zona = _context.Zona
                .Where(z => z.IdZona == id)
                .Select(z => new
                {
                    z.IdZona,
                    z.Descripcion,
                    z.IdEstado,
                    
                    Estado = z.IdEstadoNavigation != null ? z.IdEstadoNavigation.Descripcion : null,
                    
                })
                .FirstOrDefault();

            if (zona == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return Json(zona);
        }

        //ACTUALIZAR ZONA

        [HttpPost]
        public IActionResult ActualizarZona([FromBody] Zona zona)
        {
            try
            {
                if (zona == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío" });
                }

                var zonaExistente = _context.Zona
                    .Where(z => z.IdZona == zona.IdZona)
                    .FirstOrDefault();

                if (zonaExistente == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                // Actualizar los datos del usuario
                zonaExistente.Descripcion = zona.Descripcion;
               
                zonaExistente.IdEstado = zona.IdEstado;
                

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                return Ok(new { message = "Usuario actualizado correctamente" });
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
