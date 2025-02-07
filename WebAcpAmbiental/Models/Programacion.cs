using System;
using System.Collections.Generic;

namespace WebAcpAmbiental.Models;

public partial class Programacion
{
    public int IdProgramacion { get; set; }

    public int IdCliente { get; set; }

    public string? Dia { get; set; }

    public string? Horario { get; set; }

    public int? IdGestion { get; set; }

    public string? Estado { get; set; }

    public DateTime? ProximaVisita { get; set; }

    public string? Comentario { get; set; }

    public string? Info01 { get; set; }

    public string? Info02 { get; set; }

    public string? Info03 { get; set; }

    public string? Info04 { get; set; }

    public virtual MaestroCliente IdClienteNavigation { get; set; } = null!;

    public virtual Gestion? IdGestionNavigation { get; set; }
}
