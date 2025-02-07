using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebAcpAmbiental.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebAcpAmbiental.Controllers

{
    
    public class UsuarioController : Controller
    {
        private readonly AcpAmbientalContext _context;

        private readonly HttpClient _httpClient;

        public UsuarioController(AcpAmbientalContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }
        public IActionResult Usuario()

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
            var usuarios = _context.Usuario
                .Include(u => u.IdEstadoNavigation)
                .Include(u => u.IdRolNavigation)
                .ToList();

            // Pasar la lista de usuarios a la vista
            return View(usuarios);
            //return View("~/Views/Usuario/Usuario.cshtml"); 
        }

        [HttpPost]
        [Route("Usuario/Create")]
        public IActionResult Create([FromBody] Usuario usuarios)
        {
            try
            {
                _context.Usuario.Add(usuarios);
                _context.SaveChanges();
                return Json(new { success = true, usuario = usuarios });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        [Route("Usuario/BuscarPorDNI/{dni}")]
        public async Task<IActionResult> BuscarPorDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni) || dni.Length != 8)
            {
                return BadRequest(new { message = "DNI inválido. Debe tener 8 dígitos." });
            }

            string url = $"https://api.apis.net.pe/v2/reniec/dni?numero={dni}";
            string token = "apis-token-12318.bBlZHOiByERv7QtKdMXXs8OXOvqMZaat";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Headers.Add("Accept", "application/json");

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound(new { message = "No se encontró información para el DNI proporcionado." });
                }

                var responseData = await response.Content.ReadAsStringAsync();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al consultar la API externa.", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("Usuario/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                // Buscar el usuario en la base de datos
                var usuario = _context.Usuario.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado." });
                }

                // Eliminar el usuario
                _context.Usuario.Remove(usuario);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerUsuario(int id)
        {
            var usuario = _context.Usuario
                .Where(u => u.IdUsuario == id)
                .Select(u => new
                {
                    u.IdUsuario,
                    u.NombreUsuario,
                    u.NrodniUsuario,
                    u.ApellidopUsuario,
                    u.FechNacimiento,
                    u.ApellidomUsuario,
                    u.UserUsuario,
                    u.PasswordUsuario,
                    u.IdEstado,
                    u.IdRol,
                    Estado = u.IdEstadoNavigation != null ? u.IdEstadoNavigation.Descripcion : null,
                    Rol = u.IdRolNavigation != null ? u.IdRolNavigation.Descripcion : null
                })
                .FirstOrDefault();

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return Json(usuario);
        }


        [HttpPost]
        public IActionResult ActualizarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío" });
                }

                var usuarioExistente = _context.Usuario
                    .Where(u => u.IdUsuario == usuario.IdUsuario)
                    .FirstOrDefault();

                if (usuarioExistente == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                // Actualizar los datos del usuario
               
                usuarioExistente.NrodniUsuario = usuario.NrodniUsuario;
                usuarioExistente.NombreUsuario = usuario.NombreUsuario;
                usuarioExistente.ApellidopUsuario = usuario.ApellidopUsuario;
                usuarioExistente.ApellidomUsuario = usuario.ApellidomUsuario;
               
                usuarioExistente.UserUsuario = usuario.UserUsuario;
                usuarioExistente.PasswordUsuario = usuario.PasswordUsuario;
                usuarioExistente.IdEstado = usuario.IdEstado;
                usuarioExistente.IdRol = usuario.IdRol;

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
