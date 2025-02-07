using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Vendedor
{
    public int IdVendedor { get; set; }

    public string DniVendedor { get; set; } = null!;

    public string NombreVendedor { get; set; } = null!;

    public string ApellidopVendedor { get; set; } = null!;

    public string ApellidomVendedor { get; set; } = null!;

    public DateTime FechnacVendedor { get; set; }

    public int? IdEstado { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual ICollection<MaestroCliente> MaestroClientes { get; set; } = new List<MaestroCliente>();
}
