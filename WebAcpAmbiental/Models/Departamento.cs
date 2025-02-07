using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string NombreDepartamento { get; set; } = null!;

    public virtual ICollection<DireccionAtencion> DireccionAtencions { get; set; } = new List<DireccionAtencion>();

    public virtual ICollection<MaestroCliente> MaestroClientes { get; set; } = new List<MaestroCliente>();

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
