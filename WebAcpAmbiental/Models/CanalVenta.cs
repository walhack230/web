using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class CanalVenta
{
    public int IdCanal { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? IdEstado { get; set; }

    public virtual ICollection<Asignacion> Asignacions { get; set; } = new List<Asignacion>();

    public virtual Estado? IdEstadoNavigation { get; set; }
}
