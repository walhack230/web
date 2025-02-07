using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class ProgramacionController : Controller
    {


        private readonly AcpAmbientalContext _context;

        private readonly HttpClient _httpClient;

        public ProgramacionController(AcpAmbientalContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Programacion()
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

            // Obtener la asignación de canal, zona y ruta del usuario
            var asignacionUsuario = _context.Asignacion
                .Where(a => a.IdUsuarioNavigation.UserUsuario == usuario)
                .Include(a => a.IdCanalNavigation)
                .Include(a => a.IdZonaNavigation)
                .Include(a => a.IdRutaNavigation)
                .Include(a => a.IdUsuarioNavigation)
                .FirstOrDefault();

            // Obtener el idUsuario desde la asignación del usuario
            if (asignacionUsuario != null)
            {
                ViewBag.IdUsuario = asignacionUsuario.IdUsuarioNavigation.IdUsuario; // Asignar el IdUsuario al ViewBag
                                                                                     // Otros valores como Canal, Zona, etc.
            }

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

            // Recuperar la programación de clientes filtrada según el rol
            List<Programacion> programacion;

            if (rol == "Vendedor" && asignacionUsuario != null)
            {
                // Mostrar solo la programación asociada a los clientes asignados al usuario
                programacion = _context.Programacion
                    .Include(p => p.IdClienteNavigation)
                    .Include(p => p.IdGestionNavigation)
                    .Where(p => p.IdClienteNavigation.IdAsignacion == asignacionUsuario.IdAsignacion)
                    .ToList();
            }
            else
            {
                // Mostrar toda la programación sin filtrar (para roles como Jefe)
                programacion = _context.Programacion
                    .Include(p => p.IdClienteNavigation)
                    .Include(p => p.IdGestionNavigation)
                    .ToList();
            }


            // Obtener todos los departamentos para el ComboBox
            var gto = _context.Gestion
                .Select(d => new { d.IdGestion, d.Descripcion })
                .ToList();
            ViewBag.Gestion = gto;

            return View(programacion);
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

        public IActionResult GetVendedores()
        {
            try
            {
                var vendedores = _context.Usuario
                    .Where(u => u.IdRolNavigation.Descripcion == "Vendedor") // Asegúrate de que el rol se llama "Vendedor"
                    .Select(u => new
                    {
                        u.IdUsuario,
                        NombreCompleto = $"{u.NombreUsuario} {u.ApellidopUsuario} {u.ApellidomUsuario}".Trim()
                    })
                    .ToList();

                return Ok(vendedores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        public IActionResult GetDistritosPorVendedor(int idVendedor)
        {
            try
            {
                var distritosProgramados = _context.Programacion
                    .Select(p => p.IdClienteNavigation.IdDistrito)
                    .Distinct()
                    .ToList();

                var distritos = _context.MaestroCliente
                    .Where(c => c.IdAsignacionNavigation.IdUsuario == idVendedor && !distritosProgramados.Contains(c.IdDistrito))
                    .GroupBy(c => c.IdDistritoNavigation.NombreDistrito)
                    .Select(g => new
                    {
                        Distrito = g.Key,
                        CantidadClientes = g.Count()
                    })
                    .ToList();

                return Ok(distritos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

     




        [HttpGet]
        public IActionResult ObtenerIdDistritoPorNombre(string nombreDistrito)
        {
            Console.WriteLine($"Parámetro recibido: {nombreDistrito}");
            var distrito = _context.Distrito.FirstOrDefault(d => d.NombreDistrito.ToLower().Trim() == nombreDistrito.ToLower().Trim());

            if (distrito == null)
            {
                Console.WriteLine("Distrito no encontrado.");
                return NotFound("Distrito no encontrado.");
            }

            Console.WriteLine($"Distrito encontrado: {distrito.NombreDistrito}, ID: {distrito.IdDistrito}");
            return Ok(new { IdDistrito = distrito.IdDistrito });
        }



        [HttpPost]
        public IActionResult GuardarProgramacion([FromBody] List<ProgramacionRequest> registros)
        {
            if (registros == null || !registros.Any())
            {
                return BadRequest("No se recibieron registros válidos.");
            }

            try
            {
                var programaciones = new List<Programacion>();

                // Total de clientes necesarios para todas las programaciones del mismo distrito
                var clientesRequeridosPorDistrito = registros
                    .GroupBy(r => r.IdDistrito)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(r => r.CantidadClientes)
                    );

                // Obtener clientes únicos para cada distrito
                var clientesPorDistrito = clientesRequeridosPorDistrito.ToDictionary(
                    kvp => kvp.Key,
                    kvp => _context.MaestroCliente
                        .Where(c => c.IdDistrito == kvp.Key && c.Estado == "Activo"
                                    && !_context.Programacion.Any(p => p.IdCliente == c.IdCliente))
                        .OrderBy(c => c.IdCliente)
                        .Take(kvp.Value)
                        .ToList()
                );

                foreach (var request in registros)
                {
                    if (request.CantidadClientes <= 0 || string.IsNullOrWhiteSpace(request.Dia) || !request.IdDistrito.HasValue)
                    {
                        return BadRequest("Datos inválidos.");
                    }

                    var idDistrito = request.IdDistrito.Value;

                    if (!clientesPorDistrito.ContainsKey(idDistrito) || clientesPorDistrito[idDistrito].Count < request.CantidadClientes)
                    {
                        return BadRequest($"No hay suficientes clientes activos disponibles para programar en el distrito {idDistrito}.");
                    }

                    // Obtener la cantidad de clientes necesaria para este día
                    var clientesSeleccionados = clientesPorDistrito[idDistrito].Take(request.CantidadClientes).ToList();
                    clientesPorDistrito[idDistrito].RemoveRange(0, request.CantidadClientes);

                    var diaActual = DateTime.Parse(request.Dia).ToString("yyyy-MM-dd");
                    foreach (var cliente in clientesSeleccionados.Select((c, index) => new { Cliente = c, Index = index }))
                    {
                        programaciones.Add(new Programacion
                        {
                            IdCliente = cliente.Cliente.IdCliente,
                            Dia = diaActual,
                            Horario = new TimeSpan(8, 30, 0).Add(TimeSpan.FromMinutes(10 * cliente.Index)).ToString(@"hh\:mm"),
                            Estado = "Pendiente"
                        });

                        // Cambiar el estado del cliente a "Programado"
                        cliente.Cliente.Estado = "Programado";
                    }
                }

                // Guardar los registros en la base de datos
                _context.Programacion.AddRange(programaciones);
                _context.SaveChanges();

                return Ok("Programación registrada exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }



        public class ProgramacionRequest
        {
            public string Dia { get; set; }
            public int? IdDistrito { get; set; }
            public int CantidadClientes { get; set; }
        }



        [HttpGet]
        public IActionResult ObtenerProgramacion(int idProgramacion)
        {
            var programacion = _context.Programacion
                .Where(r => r.IdProgramacion == idProgramacion)
                .Select(r => new
                {
                    r.IdProgramacion,
                    r.ProximaVisita,
                    r.Comentario,
                    IdGestion = r.IdGestionNavigation != null ? r.IdGestionNavigation.IdGestion : (int?)null, // Enviar ID
                })
                .FirstOrDefault();

            if (programacion == null)
            {
                return NotFound(new { mensaje = "Programacion no encontrada" });
            }

            return Json(programacion);
        }

        [HttpPost]
        public IActionResult ActualizarProgramacion([FromBody] Programacion programacion)
        {
            try
            {
                // Verifica que el cuerpo de la solicitud no esté vacío
                if (programacion == null)
                {
                    return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío" });
                }

                // Busca la programación existente en la base de datos
                var programacionExistente = _context.Programacion
                    .FirstOrDefault(r => r.IdProgramacion == programacion.IdProgramacion);

                // Si no se encuentra la programación, devuelve error
                if (programacionExistente == null)
                {
                    return NotFound(new { message = "Programacion no encontrada" });
                }

                // Verifica si ProximaVisita tiene un valor y si es válido
                if (programacion.ProximaVisita == null) // Si ProximaVisita es null
                {
                    programacionExistente.ProximaVisita = null;  // Asigna null si no hay próxima visita
                }
                else
                {
                    programacionExistente.ProximaVisita = programacion.ProximaVisita.Value; // Si tiene valor, lo asigna
                }

                //// Si no tiene valor, puedes asignar la fecha actual
                //if (!programacion.ProximaVisita.HasValue)
                //{
                //    programacionExistente.ProximaVisita = DateTime.Now;  // Asigna la fecha actual si es null
                //}


                // Asigna los valores para Comentario y IdGestion (si están disponibles)
                programacionExistente.Comentario = programacion.Comentario ?? "Sin comentarios";
                programacionExistente.IdGestion = programacion.IdGestion;

                // Guarda los cambios en la base de datos
                _context.SaveChanges();

                return Ok(new { message = "Programacion actualizada correctamente" });
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devuelve un mensaje adecuado
                return StatusCode(500, new { message = "Ocurrió un error en el servidor", error = ex.Message });
            }
        }


        [HttpGet("GetProgramaciones")]
        public async Task<IActionResult> GetProgramaciones([FromQuery] int idUsuario)
        {
            var programaciones = await _context.Programacion
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdClienteNavigation.IdAsignacionNavigation.IdUsuarioNavigation)
                .Include(p => p.IdClienteNavigation.IdDistritoNavigation)
                .Where(p => p.IdClienteNavigation.IdAsignacionNavigation.IdUsuarioNavigation.IdUsuario == idUsuario) // Filtrar por ID
                .Select(p => new
                {
                    Vendedor = p.IdClienteNavigation.IdAsignacionNavigation.IdUsuarioNavigation.NombreUsuario + " " +
                               p.IdClienteNavigation.IdAsignacionNavigation.IdUsuarioNavigation.ApellidopUsuario,
                    Dia = p.Dia,
                    Distrito = p.IdClienteNavigation.IdDistritoNavigation.NombreDistrito
                })
                .ToListAsync();

            var resultado = programaciones
                .GroupBy(p => new { p.Dia, p.Distrito })
                .Select(g => new
                {
                    Dia = g.Key.Dia,
                    Distrito = g.Key.Distrito,
                    Cantidad = g.Count()
                })
                .OrderBy(r => r.Dia)
                .ThenBy(r => r.Distrito)
                .ToList();

            return Ok(resultado);
        }




    }
}
