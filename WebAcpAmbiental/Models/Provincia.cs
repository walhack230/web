using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Provincia
{
    public int IdProvincia { get; set; }

    public string NombreProvincia { get; set; } = null!;

    public int IdDepartamento { get; set; }

    public virtual ICollection<DireccionAtencion> DireccionAtencions { get; set; } = new List<DireccionAtencion>();

    public virtual ICollection<Distrito> Distritos { get; set; } = new List<Distrito>();

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<MaestroCliente> MaestroClientes { get; set; } = new List<MaestroCliente>();
}
