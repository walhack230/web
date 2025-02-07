using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class MaestroCliente
{
    public int IdCliente { get; set; }

    public string? CodigoCliente { get; set; }

    public string? TipoCliente { get; set; }

    public string? DocumentoCliente { get; set; }

    public string? NrodocumentoCliente { get; set; }

    public string? NombreCliente { get; set; }

    public string? ApellidopCliente { get; set; }

    public string? ApellidomCliente { get; set; }

    public string? RazonsocialCliente { get; set; }

    public string? ViadireccionCliente { get; set; }

    public string? CalledireccionCliente { get; set; }

    public string? NrodireccionCliente { get; set; }

    public string? UrbdireccionCliente { get; set; }

    public string? CodpostaldireccionCliente { get; set; }

    public string? TelefonoCliente { get; set; }

    public string? CelularCliente { get; set; }

    public string? EmailCliente { get; set; }

    public string? DocumentoRepresentante { get; set; }

    public string? NrodocumentoRepresentante { get; set; }

    public string? NombreRepresentante { get; set; }

    public string? ApellidopRepresentante { get; set; }

    public string? ApellidomRepresentante { get; set; }

    public string? CargoRepresentante { get; set; }

    public string? TelefonoRepresentante { get; set; }

    public string? CelularRepresentante { get; set; }

    public string? EmailempresarialRepresentante { get; set; }

    public string? EmailpersonalRepresentante { get; set; }

    public string? DocumentoContacto { get; set; }

    public string? NrodocumentoContacto { get; set; }

    public string? NombreContacto { get; set; }

    public string? ApellidopContacto { get; set; }

    public string? ApellidomContacto { get; set; }

    public string? CargoContacto { get; set; }

    public string? TelefonoContacto { get; set; }

    public string? CelularContacto { get; set; }

    public string? EmailempresarialContacto { get; set; }

    public string? EmailpersonalContacto { get; set; }

    public int? IdDepartamento { get; set; }

    public int? IdProvincia { get; set; }

    public int? IdDistrito { get; set; }

    public int? IdAsignacion { get; set; }

    public string? ActividadPrincipalCliente { get; set; }

    public string? VentanaAtencionCliente { get; set; }

    public string? TipoContacto { get; set; }

    public string? Estado { get; set; }

    public string NombreCompleto
    {
        get
        {
            if (TipoCliente?.ToUpper() == "JURIDICO")
            {
                return RazonsocialCliente ?? "Razón Social no disponible";
            }
            else
            {
                return $"{NombreCliente} {ApellidopCliente} {ApellidomCliente}".Trim();
            }
        }
    }



    public virtual ICollection<DireccionAtencion> DireccionAtencions { get; set; } = new List<DireccionAtencion>();

    public virtual Asignacion? IdAsignacionNavigation { get; set; }

    public virtual Departamento? IdDepartamentoNavigation { get; set; }

    public virtual Distrito? IdDistritoNavigation { get; set; }

    public virtual Provincia? IdProvinciaNavigation { get; set; }

    public virtual ICollection<Programacion> Programacions { get; set; } = new List<Programacion>();
}
