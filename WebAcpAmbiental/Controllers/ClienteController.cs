using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class ClienteController : Controller
    {


        private readonly AcpAmbientalContext _context;

        private readonly HttpClient _httpClient;

        public ClienteController(AcpAmbientalContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
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

        public IActionResult Cliente()
        {
            // Obtener el nombre de usuario y rol de la sesión
            var usuario = HttpContext.Session.GetString("Usuario");
            var rol = HttpContext.Session.GetString("Rol");

            // Verificar si se encontró un usuario y rol en la sesión
            if (usuario != null && rol != null)
            {
                ViewBag.Usuario = usuario;
                ViewBag.Rol = rol;
            }

            // Obtener la asignación de canal, zona y ruta del usuario (suponiendo que el usuario tiene una asignación)
            var asignacionUsuario = _context.Asignacion
                .Where(a => a.IdUsuarioNavigation.UserUsuario == usuario)
                .Include(a => a.IdCanalNavigation)
                .Include(a => a.IdZonaNavigation)
                .Include(a => a.IdRutaNavigation)
                .Include(a => a.IdUsuarioNavigation)
                .FirstOrDefault();

            if (asignacionUsuario != null)
            {
                ViewBag.Canal = asignacionUsuario.IdCanalNavigation.Descripcion;
                ViewBag.Zona = asignacionUsuario.IdZonaNavigation.Descripcion;
                ViewBag.Ruta = asignacionUsuario.IdRutaNavigation.Descripcion;
                ViewBag.Usuario = asignacionUsuario.IdUsuarioNavigation.NombreUsuario;
                ViewBag.IdAsignacion = asignacionUsuario.IdAsignacion; // Capturar y pasar el ID de la asignación
            }
            else
            {
                ViewBag.Canal = "No asignado";
                ViewBag.Zona = "No asignado";
                ViewBag.Ruta = "No asignado";
                ViewBag.IdAsignacion = null; // Establecer como nulo si no hay asignación
            }

            // Obtener todos los departamentos para el ComboBox
            var departamentos = _context.Departamento
                .Select(d => new { d.IdDepartamento, d.NombreDepartamento })
                .ToList();
            ViewBag.Departamentos = departamentos;

            // Recuperar los usuarios y sus relaciones
            var rutas = _context.Ruta
                .Include(u => u.IdEstadoNavigation)
                .ToList();

            // Obtener la lista de clientes filtrada según el rol
            List<MaestroCliente> clientes;

            if (rol == "Vendedor")
            {
                // Mostrar solo los clientes asignados al vendedor (filtrados por la asignación)
                clientes = _context.MaestroCliente
                    .Include(c => c.IdDepartamentoNavigation)
                    .Include(c => c.IdProvinciaNavigation)
                    .Include(c => c.IdDistritoNavigation)
                    .Where(c => c.IdAsignacion == asignacionUsuario.IdAsignacion) // Filtrar por asignación
                    .ToList();
            }
            else // Rol de Jefe o cualquier otro
            {
                // Mostrar todos los clientes (sin filtrar por asignación)
                clientes = _context.MaestroCliente
                    .Include(c => c.IdDepartamentoNavigation)
                    .Include(c => c.IdProvinciaNavigation)
                    .Include(c => c.IdDistritoNavigation)
                    .ToList();
            }

            return View(clientes);
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

        // Endpoint para obtener los vendedores
  


        [HttpGet]
        public IActionResult GetProvincias(int departamentoId)
        {
            var provincias = _context.Provincia
                .Where(p => p.IdDepartamento == departamentoId)
                .Select(p => new { p.IdProvincia, p.NombreProvincia })
                .ToList();

            return Json(provincias);
        }

        [HttpGet]
        public IActionResult GetDistritos(int provinciaId)
        {
            var distritos = _context.Distrito
                .Where(d => d.IdProvincia == provinciaId)
                .Select(d => new { d.IdDistrito, d.NombreDistrito })
                .ToList();

            return Json(distritos);
        }



        [HttpGet]
        [Route("BuscarPorDNI/{dni}")]
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

        [HttpGet]
        [Route("BuscarPorRUC/{ruc}")]
        public async Task<IActionResult> BuscarPorRUC(string ruc)
        {
            if (string.IsNullOrWhiteSpace(ruc) || ruc.Length != 11)
            {
                return BadRequest(new { message = "RUC inválido. Debe tener 11 dígitos." });
            }

            string url = $"https://api.apis.net.pe/v2/sunat/ruc?numero={ruc}";
            string token = "apis-token-12318.bBlZHOiByERv7QtKdMXXs8OXOvqMZaat";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Headers.Add("Accept", "application/json");

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound(new { message = "No se encontró información para el RUC proporcionado." });
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
        public IActionResult Create([FromBody] MaestroCliente maestroCliente)
        {
            try
            {
                // Verificar si el DNI o RUC ya está registrado
                var clienteExistente = _context.MaestroCliente
                    .FirstOrDefault(c => c.NrodocumentoCliente == maestroCliente.NrodocumentoCliente);

                if (clienteExistente != null)
                {
                    return Json(new { success = false, message = "El DNI o RUC ya está registrado." });
                }

                // Guardar el cliente si no está repetido
                _context.MaestroCliente.Add(maestroCliente);
                _context.SaveChanges();

                return Json(new { success = true, message = "Cliente registrado con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        [HttpPost]
        [Route("Cliente/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                // Buscar el cliente en la base de datos
                var cliente = _context.MaestroCliente
                    .FirstOrDefault(u => u.IdCliente == id);

                if (cliente == null)
                {
                    return Json(new { success = false, message = "Cliente no encontrado." });
                }

                // Verificar si el cliente tiene direcciones asociadas
                var direcciones = _context.DireccionAtencion
                    .Where(d => d.IdCliente == id)
                    .ToList();

                if (direcciones.Any()) // Si tiene direcciones, no se puede eliminar
                {
                    return Json(new { success = false, message = "Este cliente no se puede eliminar ya que tiene direcciones de atención registradas." });
                }

                // Eliminar el cliente si no tiene direcciones asociadas
                _context.MaestroCliente.Remove(cliente);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public IActionResult ObtenerMaestroCliente(int id)
        {
            var maestrocliente = _context.MaestroCliente
                 .Include(u => u.IdDepartamentoNavigation)
        .Include(u => u.IdProvinciaNavigation)
        .Include(u => u.IdDistritoNavigation)
        .Include(u => u.IdAsignacionNavigation)
            .ThenInclude(a => a.IdCanalNavigation)
        .Include(u => u.IdAsignacionNavigation)
            .ThenInclude(a => a.IdZonaNavigation)
        .Include(u => u.IdAsignacionNavigation)
            .ThenInclude(a => a.IdRutaNavigation)
        .Include(u => u.IdAsignacionNavigation)
            .ThenInclude(a => a.IdUsuarioNavigation)
                .Where(u => u.IdCliente == id)
                .Select(u => new
                {

                    //CLIENTE
                    u.IdCliente,
                    u.CodigoCliente,
                    u.TipoCliente,
                    u.DocumentoCliente,
                    u.NrodocumentoCliente,
                    u.NombreCliente,
                    u.ApellidopCliente,
                    u.ApellidomCliente,
                    u.RazonsocialCliente,
                    u.ViadireccionCliente,
                    u.CalledireccionCliente,
                    u.NrodireccionCliente,
                    u.UrbdireccionCliente,
                    u.CodpostaldireccionCliente,
                    u.TelefonoCliente,
                    u.CelularCliente,
                    u.EmailCliente,
                    u.TipoContacto,
                    u.VentanaAtencionCliente,
                    u.ActividadPrincipalCliente,
                    //Representante
                    u.DocumentoRepresentante,
                    u.NrodocumentoRepresentante,
                    u.NombreRepresentante,
                    u.ApellidopRepresentante,
                    u.ApellidomRepresentante,
                    u.CargoRepresentante,
                    u.TelefonoRepresentante,
                    u.CelularRepresentante,
                    u.EmailempresarialRepresentante,
                    u.EmailpersonalRepresentante,

                    //CONTACTO
                    u.DocumentoContacto,
                    u.NrodocumentoContacto,
                    u.NombreContacto,
                    u.ApellidopContacto,
                    u.ApellidomContacto,
                    u.CargoContacto,
                    u.TelefonoContacto,
                    u.CelularContacto,
                    u.EmailempresarialContacto,
                    u.EmailpersonalContacto,


                    u.IdDepartamento,
                    u.IdProvincia,
                    u.IdDistrito,
                    u.IdAsignacion,
                    Departamento = u.IdDepartamentoNavigation != null ? u.IdDepartamentoNavigation.NombreDepartamento : null,
                    Provincia = u.IdProvinciaNavigation != null ? u.IdProvinciaNavigation.NombreProvincia : null,
                     Distrito = u.IdDistritoNavigation != null ? u.IdDistritoNavigation.NombreDistrito : null,
                    Canal = u.IdAsignacionNavigation != null ? u.IdAsignacionNavigation.IdCanalNavigation.Descripcion : null,
                    Zona = u.IdAsignacionNavigation != null ? u.IdAsignacionNavigation.IdZonaNavigation.Descripcion : null,
                    Ruta = u.IdAsignacionNavigation != null ? u.IdAsignacionNavigation.IdRutaNavigation.Descripcion : null,
                    Usuario = u.IdAsignacionNavigation != null ? u.IdAsignacionNavigation.IdUsuarioNavigation.NombreUsuario : null
                })
                .FirstOrDefault();

            if (maestrocliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            return Json(maestrocliente);
        }





        public JsonResult GetVendedores()
        {
            var vendedores = _context.Usuario
                .Where(u => u.IdRolNavigation.Descripcion == "Vendedor")
                .Select(u => new
                {
                    u.IdUsuario,
                    NombreCompleto = $"{u.NombreUsuario} {u.ApellidopUsuario} {u.ApellidomUsuario}"
                })
                .ToList();

            return Json(vendedores);
        }

        public IActionResult GetAsignacionesByUserId(int userId)
        {
            var asignaciones = _context.Asignacion
                .Where(a => a.IdUsuario == userId)
                .Select(a => new
                {
                    idAsignacion = a.IdAsignacion,  // Incluye el IdAsignacion
                    CanalDescripcion = a.IdCanalNavigation.Descripcion,
                    ZonaDescripcion = a.IdZonaNavigation.Descripcion,
                    RutaDescripcion = a.IdRutaNavigation.Descripcion
                })
                .ToList();

            if (asignaciones == null || !asignaciones.Any())
            {
                return NotFound(new { message = "No se encontraron asignaciones para el usuario especificado." });
            }

            return Ok(asignaciones);
        }




        [HttpPost]
        public IActionResult ActualizarMaestroCliente([FromBody] MaestroCliente maestrocliente)
        {
            try
            {
                if (maestrocliente == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío" });
                }

                var maestroclienteExistente = _context.MaestroCliente
                    .Where(u => u.IdCliente == maestrocliente.IdCliente)
                    .FirstOrDefault();

                if (maestroclienteExistente == null)
                {
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                // Actualizar los datos del usuario


                maestroclienteExistente.CodigoCliente = maestrocliente.CodigoCliente;
                maestroclienteExistente.TipoCliente = maestrocliente.TipoCliente;
                maestroclienteExistente.DocumentoCliente = maestrocliente.DocumentoCliente;
                maestroclienteExistente.NrodocumentoCliente = maestrocliente.NrodocumentoCliente;
                maestroclienteExistente.NombreCliente = maestrocliente.NombreCliente;
                maestroclienteExistente.ApellidopCliente = maestrocliente.ApellidopCliente;
                maestroclienteExistente.ApellidomCliente = maestrocliente.ApellidomCliente;
                maestroclienteExistente.RazonsocialCliente = maestrocliente.RazonsocialCliente;
                maestroclienteExistente.ViadireccionCliente = maestrocliente.ViadireccionCliente;
                maestroclienteExistente.CalledireccionCliente = maestrocliente.CalledireccionCliente;
                maestroclienteExistente.NrodireccionCliente = maestrocliente.NrodireccionCliente;
                maestroclienteExistente.UrbdireccionCliente = maestrocliente.UrbdireccionCliente;
                maestroclienteExistente.CodpostaldireccionCliente = maestrocliente.CodpostaldireccionCliente;
                maestroclienteExistente.TelefonoCliente = maestrocliente.TelefonoCliente;
                maestroclienteExistente.CelularCliente = maestrocliente.CelularCliente;
                maestroclienteExistente.EmailCliente = maestrocliente.EmailCliente;

                //REPRESENTANTE
                maestroclienteExistente.DocumentoRepresentante = maestrocliente.DocumentoRepresentante;
                maestroclienteExistente.NrodocumentoRepresentante = maestrocliente.NrodocumentoRepresentante;
                maestroclienteExistente.NombreRepresentante = maestrocliente.NombreRepresentante;
                maestroclienteExistente.ApellidopRepresentante = maestrocliente.ApellidopRepresentante;
                maestroclienteExistente.ApellidomRepresentante = maestrocliente.ApellidomRepresentante;
                maestroclienteExistente.CargoRepresentante = maestrocliente.CargoRepresentante;
                maestroclienteExistente.TelefonoRepresentante = maestrocliente.TelefonoRepresentante;
                maestroclienteExistente.CelularRepresentante = maestrocliente.CelularRepresentante;
                maestroclienteExistente.EmailempresarialRepresentante = maestrocliente.EmailempresarialRepresentante;
                maestroclienteExistente.EmailpersonalRepresentante = maestrocliente.EmailpersonalRepresentante;


                //CONTACTO
                maestroclienteExistente.DocumentoContacto = maestrocliente.DocumentoContacto;
                maestroclienteExistente.NrodocumentoContacto = maestrocliente.NrodocumentoContacto;
                maestroclienteExistente.NombreContacto = maestrocliente.NombreContacto;
                maestroclienteExistente.ApellidopContacto = maestrocliente.ApellidopContacto;
                maestroclienteExistente.ApellidomContacto = maestrocliente.ApellidomContacto;
                maestroclienteExistente.CargoContacto = maestrocliente.CargoContacto;
                maestroclienteExistente.TelefonoContacto = maestrocliente.TelefonoContacto;
                maestroclienteExistente.CelularContacto = maestrocliente.CelularContacto;
                maestroclienteExistente.EmailempresarialContacto = maestrocliente.EmailempresarialContacto;
                maestroclienteExistente.EmailpersonalContacto = maestrocliente.EmailpersonalContacto;

                //EXTRA
                maestroclienteExistente.ActividadPrincipalCliente = maestrocliente.ActividadPrincipalCliente;
                maestroclienteExistente.VentanaAtencionCliente = maestrocliente.VentanaAtencionCliente;
                maestroclienteExistente.TipoContacto = maestrocliente.TipoContacto;

                //ID

                maestroclienteExistente.IdDepartamento = maestrocliente.IdDepartamento;
                maestroclienteExistente.IdProvincia = maestrocliente.IdProvincia;
                maestroclienteExistente.IdDistrito = maestrocliente.IdDistrito;
                maestroclienteExistente.IdAsignacion = maestrocliente.IdAsignacion;


              
                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                return Ok(new { message = "Cliente actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error en el servidor", error = ex.Message });
            }
        }








    }
}
