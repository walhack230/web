using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Gestion
{
    public int IdGestion { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Programacion> Programacions { get; set; } = new List<Programacion>();
}
