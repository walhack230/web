using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string UserUsuario { get; set; } = null!;

    public string PasswordUsuario { get; set; } = null!;

    public int? IdEstado { get; set; }

    public int? IdRol { get; set; }

    public string? NrodniUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? ApellidopUsuario { get; set; }

    public string? ApellidomUsuario { get; set; }

    public string? FechNacimiento { get; set; }

    public virtual ICollection<Asignacion> Asignacions { get; set; } = new List<Asignacion>();

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
