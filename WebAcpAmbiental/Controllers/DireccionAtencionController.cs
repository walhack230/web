using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAcpAmbiental.Models;

namespace WebAcpAmbiental.Controllers
{
    public class DireccionAtencionController : Controller
    {

        private readonly AcpAmbientalContext _context;

        public DireccionAtencionController(AcpAmbientalContext context)
        {
            _context = context;
        }


        public IActionResult DireccionAtencion()
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




            List<DireccionAtencion> direccionesAtencion;

            if (rol == "Vendedor")
            {
                // Mostrar solo las direcciones de atención registradas por el usuario actual
                direccionesAtencion = _context.DireccionAtencion
                    .Include(d => d.IdDepartamentoNavigation)
                    .Include(d => d.IdProvinciaNavigation)
                    .Include(d => d.IdDistritoNavigation)
                    .Include(d => d.IdClienteNavigation)
                    .Where(d => d.IdClienteNavigation.IdAsignacionNavigation.IdUsuarioNavigation.UserUsuario == usuario)
                    .ToList();
            }
            else
            {
                // Mostrar todas las direcciones de atención
                direccionesAtencion = _context.DireccionAtencion
                    .Include(d => d.IdDepartamentoNavigation)
                    .Include(d => d.IdProvinciaNavigation)
                    .Include(d => d.IdDistritoNavigation)
                    .Include(d => d.IdClienteNavigation)
                    .ToList();
            }

            return View(direccionesAtencion);

          

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

        [HttpGet]
        public JsonResult GetDepartamentos()
        {
            var departamentos = _context.Departamento
                .Select(d => new { d.IdDepartamento, d.NombreDepartamento })
                .ToList();
            return Json(departamentos);
        }

        [HttpGet]
        public JsonResult GetProvincias(int idDepartamento)
        {
            var provincias = _context.Provincia
                .Where(p => p.IdDepartamento == idDepartamento)
                .Select(p => new { p.IdProvincia, p.NombreProvincia })
                .ToList();
            return Json(provincias);
        }

        [HttpGet]
        public JsonResult GetDistritos(int idProvincia)
        {
            var distritos = _context.Distrito
                .Where(d => d.IdProvincia == idProvincia)
                .Select(d => new { d.IdDistrito, d.NombreDistrito })
                .ToList();
            return Json(distritos);
        }
        [HttpGet]
        public JsonResult GetClientes()
        {
            var clientes = _context.MaestroCliente
                .Select(c => new
                {
                    c.IdCliente,
                    Nombre = c.NombreCompleto // Uso de la propiedad calculada
                })
                .ToList();

            return Json(clientes);
        }




        [HttpPost]
        public IActionResult Create([FromBody] DireccionAtencion direccionAtencions)
        {
            try
            {
                _context.DireccionAtencion.Add(direccionAtencions);
                _context.SaveChanges();
                return Json(new { success = true, DireccionAtencion = direccionAtencions });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        [Route("DireccionAtencion/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            try
            {
               
                var direccionAtencion = _context.DireccionAtencion.FirstOrDefault(u => u.IdDireccion == id);
                if (direccionAtencion == null)
                {
                    return Json(new { success = false, message = "direccion de Atencion no encontrado." });
                }

                
                _context.DireccionAtencion.Remove(direccionAtencion);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public IActionResult ObtenerDireccionesAtencion(int id)
        {
            var direccionesAtencion = _context.DireccionAtencion
                .Where(d => d.IdDireccion == id)
                .Select(d => new
                {
                   d.IdDireccion,
                    d.ViaDireccion,
                    d.CalleDireccion,
                    d.NroDireccion,
                    d.UrbDireccion,
                    d.CodpostalDireccion,
                    d.CoordenadasDireccion,
                 

                    // Ubicación
                    d.IdDepartamento,
                    d.IdProvincia,
                    d.IdDistrito,
                    d.IdCliente,

                    Departamento = d.IdDepartamentoNavigation != null ? d.IdDepartamentoNavigation.NombreDepartamento : null,
                    Provincia = d.IdProvinciaNavigation != null ? d.IdProvinciaNavigation.NombreProvincia : null,
                    Distrito = d.IdDistritoNavigation != null ? d.IdDistritoNavigation.NombreDistrito : null,
                    Cliente = d.IdClienteNavigation != null ? d.IdClienteNavigation.NombreCompleto : null

                 

                })
           .FirstOrDefault();

            if (direccionesAtencion == null)
            {
                return NotFound(new { mensaje = "Direccion no encontrada" });
            }
            return Json(direccionesAtencion);
        }












    }



}
