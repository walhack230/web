using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class AsignacionController : Controller
    {
        private readonly AcpAmbientalContext _context;

        public AsignacionController(AcpAmbientalContext context)
        {
            _context = context;
        }

        public IActionResult Asignacion()
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
            var asignacions = _context.Asignacion
                .Include(u => u.IdCanalNavigation)
                .Include(u => u.IdRutaNavigation)
                .Include(u => u.IdZonaNavigation)
                .Include(u => u.IdUsuarioNavigation)
                .ToList();

            // Pasar la lista de usuarios a la vista
            return View(asignacions);
            //return View("~/Views/Usuario/Usuario.cshtml"); 

        }

      

        // Endpoint para obtener los canales de venta
        [HttpGet]
        public IActionResult GetCanalesVenta()
        {
            var canales = _context.CanalVenta
                                  .Where(c => c.IdEstado == 1) // Ejemplo: Solo activos
                                  .Select(c => new { c.IdCanal, c.Descripcion })
                                  .ToList();
            return Json(canales);
        }

        // Endpoint para obtener las zonas
        [HttpGet]
        public IActionResult GetZonas()
        {
            var zonas = _context.Zona
                                .Where(z => z.IdEstado == 1) // Ejemplo: Solo activas
                                .Select(z => new { z.IdZona, z.Descripcion })
                                .ToList();
            return Json(zonas);
        }

        // Endpoint para obtener las rutas
        [HttpGet]
        public IActionResult GetRutas()
        {
            var rutas = _context.Ruta
                                .Where(r => r.IdEstado == 1) // Ejemplo: Solo activas
                                .Select(r => new { r.IdRuta, r.Descripcion })
                                .ToList();
            return Json(rutas);
        }


        // Endpoint para obtener las rutas
        [HttpGet]
        public IActionResult GetUsuario()
        {
            var usuarios = _context.Usuario
                                    .Where(r => r.IdEstado == 1 && r.IdRol == 2) // Ejemplo: Solo activas
                                    .Select(r => new
                                    {
                                        r.IdUsuario,
                                        NombreCompleto = r.NombreUsuario + " " + r.ApellidopUsuario + " " + r.ApellidomUsuario
                                    })
                                    .ToList();

            return Json(usuarios);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Asignacion asignacion)
        {
            if (asignacion == null)
            {
                return BadRequest(new { success = false, message = "Datos inválidos." });
            }

            // Verificar si el usuario (vendedor) ya tiene una asignación
            var existingAsignacion = _context.Asignacion
                .Where(a => a.IdUsuario == asignacion.IdUsuario)
                .FirstOrDefault();

            if (existingAsignacion != null)
            {
                return BadRequest(new { success = false, message = "Este vendedor ya tiene una asignación registrada." });
            }

            try
            {
                _context.Asignacion.Add(asignacion);
                _context.SaveChanges();

                return Ok(new { success = true, Asignacion = asignacion });
            }
            catch (Exception ex)
            {
                // Maneja errores de base de datos o lógica de negocio
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }



        [HttpPost]
        [Route("Asignacion/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                // Buscar la asignacion en la base de datos
                var asignacions = _context.Asignacion.FirstOrDefault(u => u.IdAsignacion == id);
                if (asignacions == null)
                {
                    return Json(new { success = false, message = "Asignacion no encontrada." });
                }

                // Eliminar el usuario
                _context.Asignacion.Remove(asignacions);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public IActionResult ObtenerAsignacion(int id)
        {
            var asignacion = _context.Asignacion
                .Where(a => a.IdAsignacion == id)
                .Select(a => new
                {
                    a.IdAsignacion,
                  

                    a.IdCanal,

                    Canal = a.IdCanalNavigation != null ? a.IdCanalNavigation.Descripcion : null,

                    a.IdZona,

                    Zona = a.IdZonaNavigation != null ? a.IdZonaNavigation.Descripcion : null,

                    a.IdRuta,

                    Ruta = a.IdRutaNavigation != null ? a.IdRutaNavigation.Descripcion : null,

                    a.IdUsuario,
                    Usuario = a.IdUsuarioNavigation != null
    ? $"{a.IdUsuarioNavigation.NombreUsuario} {a.IdUsuarioNavigation.ApellidopUsuario} {a.IdUsuarioNavigation.ApellidomUsuario}"
    : null,

                })
                .FirstOrDefault();

            if (asignacion
                == null)
            {
                return NotFound(new { mensaje = "Asignacion no encontrada" });
            }

            return Json(asignacion);
        }

        [HttpPost]
        public IActionResult ActualizarAsignacion([FromBody] Asignacion asignacions)
        {
            try
            {
                if (asignacions == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío" });
                }

                var asignacionExistente = _context.Asignacion
                    .Where(r => r.IdAsignacion == asignacions.IdAsignacion)
                    .FirstOrDefault();

                if (asignacionExistente == null)
                {
                    return NotFound(new { message = "Asignacion no encontrada" });
                }

              
                asignacionExistente.IdCanal = asignacions.IdCanal;
                asignacionExistente.IdZona = asignacions.IdZona;
                asignacionExistente.IdRuta = asignacions.IdRuta;
                asignacionExistente.IdUsuario = asignacions.IdUsuario;


                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                return Ok(new { message = "Asignacion actualizada correctamente" });
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
