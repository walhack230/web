using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class ReporteController : Controller


    {

        private readonly AcpAmbientalContext _context;

        public ReporteController(AcpAmbientalContext context)
        {
            _context = context;
        }


        public IActionResult Reporte()
        {
            // Obtener el nombre de usuario y rol de la sesión
            var usuario = HttpContext.Session.GetString("Usuario");
            var rol = HttpContext.Session.GetString("Rol");

            if (usuario != null && rol != null)
            {
                ViewBag.Usuario = usuario;
                ViewBag.Rol = rol;
            }

            // Listado completo de clientes con detalles de asignación
            var listadoClientes = _context.MaestroCliente
                .Include(c => c.IdAsignacionNavigation) // Incluye la relación con Asignacion
                .ThenInclude(a => a.IdZonaNavigation)   // Incluye la Zona
                .Include(c => c.IdAsignacionNavigation.IdRutaNavigation) // Incluye la Ruta
                .Include(c => c.IdAsignacionNavigation.IdCanalNavigation) // Incluye el Canal
                .Include(c => c.IdAsignacionNavigation.IdUsuarioNavigation) // Incluye el Vendedor/Usuario
                .ToList();

            // Pasar datos a la vista
            ViewBag.ListadoClientes = listadoClientes;

            var listadoDireccionesConClientes = _context.DireccionAtencion
      .Include(d => d.IdClienteNavigation) // Relación con MaestroCliente
      .Include(d => d.IdDepartamentoNavigation) // Relación con Departamento
      .Include(d => d.IdProvinciaNavigation) // Relación con Provincia
      .Include(d => d.IdDistritoNavigation) // Relación con Distrito
      .Where(d => d.IdCliente != null) // Filtra direcciones con cliente asociado
      .Select(d => new
      {
          IdDireccion = d.IdDireccion,
          NombreCliente = d.IdClienteNavigation.TipoCliente == "juridico"
                          ? d.IdClienteNavigation.RazonsocialCliente
                          : $"{d.IdClienteNavigation.NombreCliente} {d.IdClienteNavigation.ApellidopCliente} {d.IdClienteNavigation.ApellidomCliente}",
          CalleDireccion = d.CalleDireccion,
          NombreDepartamento = d.IdDepartamentoNavigation.NombreDepartamento,
          NombreProvincia = d.IdProvinciaNavigation.NombreProvincia,
          NombreDistrito = d.IdDistritoNavigation.NombreDistrito,
          CoordenadasDireccion = d.CoordenadasDireccion
      })
      .ToList();

            // Pasar datos a la vista
            ViewBag.ListadoDireccionesConClientes = listadoDireccionesConClientes;


            // Obtener listado de programaciones con detalles de cliente y resultado
            var listadoProgramaciones = _context.Programacion
                .Include(p => p.IdClienteNavigation) // Incluye la relación con el cliente
                .Where(p => !string.IsNullOrEmpty(p.IdGestionNavigation.Descripcion)) // Filtrar solo aquellos con resultado no vacío o nulo
                .Select(p => new
                {
                    NombreCompleto = p.IdClienteNavigation.TipoCliente == "juridico"
                                    ? p.IdClienteNavigation.RazonsocialCliente
                                    : $"{p.IdClienteNavigation.NombreCliente} {p.IdClienteNavigation.ApellidopCliente} {p.IdClienteNavigation.ApellidomCliente}",
                    TipoCliente = p.IdClienteNavigation.TipoCliente,
                    DocumentoCliente = p.IdClienteNavigation.DocumentoCliente,
                    NrodocumentoCliente = p.IdClienteNavigation.NrodocumentoCliente,
                    CalledireccionCliente = _context.DireccionAtencion
                                                    .Where(d => d.IdCliente == p.IdCliente)
                                                    .Select(d => d.CalleDireccion)
                                                    .FirstOrDefault(),
                    DiaProgramado = p.Dia,
                    Resultado = p.IdGestionNavigation.Descripcion,
                    Comentario = p.Comentario
                })
                .ToList();

            ViewBag.ListadoProgramaciones = listadoProgramaciones;

            return View("~/Views/Reporte/Reporte.cshtml");
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
            var cantidadClientesParticulares = _context.MaestroCliente.Where(c => c.TipoCliente == "particular").Count();
            var cantidadClientesJuridicos = _context.MaestroCliente.Where(c => c.TipoCliente == "juridico").Count();



            // Pasar los datos a la vista
            ViewBag.CantidadClientes = cantidadClientes;
            ViewBag.CantidadVendedores = cantidadVendedores;
            ViewBag.CantidadClientesParticulares = cantidadClientesParticulares;
            ViewBag.CantidadClientesJuridicos = cantidadClientesJuridicos;



            return View("~/Views/Principal/Principal.cshtml");
        }
    }
}
