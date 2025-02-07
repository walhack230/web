using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class DireccionAtencion
{
    public int IdDireccion { get; set; }

    public string? ViaDireccion { get; set; }

    public string? CalleDireccion { get; set; }

    public string? NroDireccion { get; set; }

    public string? UrbDireccion { get; set; }

    public string? CodpostalDireccion { get; set; }

    public string? CoordenadasDireccion { get; set; }

    public string? VentanaAtencionDireccion { get; set; }

    public string? Info1 { get; set; }

    public string? Info2 { get; set; }

    public string? Info3 { get; set; }

    public string? Info4 { get; set; }

    public string? Info5 { get; set; }

    public int? IdDepartamento { get; set; }

    public int? IdProvincia { get; set; }

    public int? IdDistrito { get; set; }

    public int? IdCliente { get; set; }

    public virtual MaestroCliente? IdClienteNavigation { get; set; }

    public virtual Departamento? IdDepartamentoNavigation { get; set; }

    public virtual Distrito? IdDistritoNavigation { get; set; }

    public virtual Provincia? IdProvinciaNavigation { get; set; }
}
