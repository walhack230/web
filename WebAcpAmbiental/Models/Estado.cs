using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<CanalVenta> CanalVenta { get; set; } = new List<CanalVenta>();

    public virtual ICollection<Ruta> Ruta { get; set; } = new List<Ruta>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Zona> Zonas { get; set; } = new List<Zona>();
}
