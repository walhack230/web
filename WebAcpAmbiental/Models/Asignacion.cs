using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Asignacion
{
    public int IdAsignacion { get; set; }

    public int? IdCanal { get; set; }

    public int? IdRuta { get; set; }

    public int? IdZona { get; set; }

    public int? IdUsuario { get; set; }

    public virtual CanalVenta? IdCanalNavigation { get; set; }

    public virtual Ruta? IdRutaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual Zona? IdZonaNavigation { get; set; }

    public virtual ICollection<MaestroCliente> MaestroClientes { get; set; } = new List<MaestroCliente>();
}
