using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Distrito
{
    public int IdDistrito { get; set; }

    public string NombreDistrito { get; set; } = null!;

    public int IdProvincia { get; set; }

    public virtual ICollection<DireccionAtencion> DireccionAtencions { get; set; } = new List<DireccionAtencion>();

    public virtual Provincia IdProvinciaNavigation { get; set; } = null!;

    public virtual ICollection<MaestroCliente> MaestroClientes { get; set; } = new List<MaestroCliente>();
}
