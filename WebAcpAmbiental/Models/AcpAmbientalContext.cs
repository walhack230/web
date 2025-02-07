using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAcpAmbiental.Models;

public partial class AcpAmbientalContext : DbContext
{
    public AcpAmbientalContext()
    {
    }

    public AcpAmbientalContext(DbContextOptions<AcpAmbientalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asignacion> Asignacion { get; set; }

    public virtual DbSet<CanalVenta> CanalVenta { get; set; }

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<DireccionAtencion> DireccionAtencion { get; set; }

    public virtual DbSet<Distrito> Distrito { get; set; }

    public virtual DbSet<Estado> Estado { get; set; }

    public virtual DbSet<Gestion> Gestion { get; set; }

    public virtual DbSet<MaestroCliente> MaestroCliente { get; set; }

    public virtual DbSet<Programacion> Programacion { get; set; }

    public virtual DbSet<Provincia> Provincia { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Ruta> Ruta { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<Zona> Zona { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-H2TBG1P1;Database=ACP_AMBIENTAL;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asignacion>(entity =>
        {
            entity.HasKey(e => e.IdAsignacion).HasName("PK__Asignaci__C3F7F966FBDA3CFF");

            entity.ToTable("Asignacion");

            entity.Property(e => e.IdAsignacion).HasColumnName("id_asignacion");
            entity.Property(e => e.IdCanal).HasColumnName("id_canal");
            entity.Property(e => e.IdRuta).HasColumnName("id_ruta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdZona).HasColumnName("id_zona");

            entity.HasOne(d => d.IdCanalNavigation).WithMany(p => p.Asignacions)
                .HasForeignKey(d => d.IdCanal)
                .HasConstraintName("FK__Asignacio__id_ca__160F4887");

            entity.HasOne(d => d.IdRutaNavigation).WithMany(p => p.Asignacions)
                .HasForeignKey(d => d.IdRuta)
                .HasConstraintName("FK__Asignacio__id_ru__17036CC0");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Asignacions)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Asignacio__id_us__18EBB532");

            entity.HasOne(d => d.IdZonaNavigation).WithMany(p => p.Asignacions)
                .HasForeignKey(d => d.IdZona)
                .HasConstraintName("FK__Asignacio__id_zo__17F790F9");
        });

        modelBuilder.Entity<CanalVenta>(entity =>
        {
            entity.HasKey(e => e.IdCanal).HasName("PK__Canal_Ve__D4C01CC8BA8AD435");

            entity.ToTable("Canal_Venta");

            entity.Property(e => e.IdCanal).HasColumnName("id_canal");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.CanalVenta)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Canal_Ven__id_es__403A8C7D");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PK__Departam__64F37A16D4A7D66D");

            entity.ToTable("Departamento");

            entity.Property(e => e.IdDepartamento).HasColumnName("id_departamento");
            entity.Property(e => e.NombreDepartamento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_departamento");
        });

        modelBuilder.Entity<DireccionAtencion>(entity =>
        {
            entity.HasKey(e => e.IdDireccion).HasName("PK__Direccio__25C35D07516E78B2");

            entity.ToTable("Direccion_Atencion");

            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.CalleDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("calle_direccion");
            entity.Property(e => e.CodpostalDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("codpostal_direccion");
            entity.Property(e => e.CoordenadasDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("coordenadas_direccion");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdDepartamento).HasColumnName("id_departamento");
            entity.Property(e => e.IdDistrito).HasColumnName("id_distrito");
            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            entity.Property(e => e.Info1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("info1");
            entity.Property(e => e.Info2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("info2");
            entity.Property(e => e.Info3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("info3");
            entity.Property(e => e.Info4)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("info4");
            entity.Property(e => e.Info5)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("info5");
            entity.Property(e => e.NroDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nro_direccion");
            entity.Property(e => e.UrbDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("urb_direccion");
            entity.Property(e => e.VentanaAtencionDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ventana_atencion_direccion");
            entity.Property(e => e.ViaDireccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("via_direccion");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.DireccionAtencions)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Direccion__id_cl__25518C17");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.DireccionAtencions)
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("FK__Direccion__id_de__22751F6C");

            entity.HasOne(d => d.IdDistritoNavigation).WithMany(p => p.DireccionAtencions)
                .HasForeignKey(d => d.IdDistrito)
                .HasConstraintName("FK__Direccion__id_di__245D67DE");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.DireccionAtencions)
                .HasForeignKey(d => d.IdProvincia)
                .HasConstraintName("FK__Direccion__id_pr__236943A5");
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.HasKey(e => e.IdDistrito).HasName("PK__Distrito__65E5575D8593118C");

            entity.ToTable("Distrito");

            entity.Property(e => e.IdDistrito).HasColumnName("id_distrito");
            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            entity.Property(e => e.NombreDistrito)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_distrito");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.Distritos)
                .HasForeignKey(d => d.IdProvincia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Distrito__id_pro__5165187F");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado__86989FB2CC032468");

            entity.ToTable("Estado");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Gestion>(entity =>
        {
            entity.HasKey(e => e.IdGestion).HasName("PK__Gestion__2920AC41BC0BC79F");

            entity.ToTable("Gestion");

            entity.Property(e => e.IdGestion).HasColumnName("id_gestion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<MaestroCliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Maestro___677F38F584C5DFED");

            entity.ToTable("Maestro_Cliente");

            entity.HasIndex(e => e.NrodocumentoCliente, "UQ__Maestro___2CB19C39C4EE3E34").IsUnique();

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.ActividadPrincipalCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("actividad_principal_cliente");
            entity.Property(e => e.ApellidomCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidom_cliente");
            entity.Property(e => e.ApellidomContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidom_contacto");
            entity.Property(e => e.ApellidomRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidom_representante");
            entity.Property(e => e.ApellidopCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidop_cliente");
            entity.Property(e => e.ApellidopContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidop_contacto");
            entity.Property(e => e.ApellidopRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidop_representante");
            entity.Property(e => e.CalledireccionCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("calledireccion_cliente");
            entity.Property(e => e.CargoContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("cargo_contacto");
            entity.Property(e => e.CargoRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("cargo_representante");
            entity.Property(e => e.CelularCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("celular_cliente");
            entity.Property(e => e.CelularContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("celular_contacto");
            entity.Property(e => e.CelularRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("celular_representante");
            entity.Property(e => e.CodigoCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("codigo_cliente");
            entity.Property(e => e.CodpostaldireccionCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("codpostaldireccion_cliente");
            entity.Property(e => e.DocumentoCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("documento_cliente");
            entity.Property(e => e.DocumentoContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("documento_contacto");
            entity.Property(e => e.DocumentoRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("documento_representante");
            entity.Property(e => e.EmailCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email_cliente");
            entity.Property(e => e.EmailempresarialContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("emailempresarial_contacto");
            entity.Property(e => e.EmailempresarialRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("emailempresarial_representante");
            entity.Property(e => e.EmailpersonalContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("emailpersonal_contacto");
            entity.Property(e => e.EmailpersonalRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("emailpersonal_representante");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.IdAsignacion).HasColumnName("id_asignacion");
            entity.Property(e => e.IdDepartamento).HasColumnName("id_departamento");
            entity.Property(e => e.IdDistrito).HasColumnName("id_distrito");
            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_cliente");
            entity.Property(e => e.NombreContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_contacto");
            entity.Property(e => e.NombreRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_representante");
            entity.Property(e => e.NrodireccionCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nrodireccion_cliente");
            entity.Property(e => e.NrodocumentoCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nrodocumento_cliente");
            entity.Property(e => e.NrodocumentoContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nrodocumento_contacto");
            entity.Property(e => e.NrodocumentoRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nrodocumento_representante");
            entity.Property(e => e.RazonsocialCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("razonsocial_cliente");
            entity.Property(e => e.TelefonoCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("telefono_cliente");
            entity.Property(e => e.TelefonoContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("telefono_contacto");
            entity.Property(e => e.TelefonoRepresentante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("telefono_representante");
            entity.Property(e => e.TipoCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipo_cliente");
            entity.Property(e => e.TipoContacto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipo_contacto");
            entity.Property(e => e.UrbdireccionCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("urbdireccion_cliente");
            entity.Property(e => e.VentanaAtencionCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ventana_atencion_cliente");
            entity.Property(e => e.ViadireccionCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("viadireccion_cliente");

            entity.HasOne(d => d.IdAsignacionNavigation).WithMany(p => p.MaestroClientes)
                .HasForeignKey(d => d.IdAsignacion)
                .HasConstraintName("FK__Maestro_C__id_as__1F98B2C1");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.MaestroClientes)
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("FK__Maestro_C__id_de__1CBC4616");

            entity.HasOne(d => d.IdDistritoNavigation).WithMany(p => p.MaestroClientes)
                .HasForeignKey(d => d.IdDistrito)
                .HasConstraintName("FK__Maestro_C__id_di__1EA48E88");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.MaestroClientes)
                .HasForeignKey(d => d.IdProvincia)
                .HasConstraintName("FK__Maestro_C__id_pr__1DB06A4F");
        });

        modelBuilder.Entity<Programacion>(entity =>
        {
            entity.HasKey(e => e.IdProgramacion).HasName("PK__Programa__3B2418FB44F420B0");

            entity.ToTable("Programacion");

            entity.HasIndex(e => new { e.IdCliente, e.Dia, e.Horario }, "unique_programacion").IsUnique();

            entity.Property(e => e.IdProgramacion).HasColumnName("id_programacion");
            entity.Property(e => e.Comentario)
                .HasColumnType("text")
                .HasColumnName("comentario");
            entity.Property(e => e.Dia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dia");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Horario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("horario");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdGestion).HasColumnName("id_gestion");
            entity.Property(e => e.Info01)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("info01");
            entity.Property(e => e.Info02)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("info02");
            entity.Property(e => e.Info03)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("info03");
            entity.Property(e => e.Info04)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("info04");
            entity.Property(e => e.ProximaVisita)
                .HasColumnType("datetime")
                .HasColumnName("proxima_visita");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Programacions)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Programacion_Cliente");

            entity.HasOne(d => d.IdGestionNavigation).WithMany(p => p.Programacions)
                .HasForeignKey(d => d.IdGestion)
                .HasConstraintName("FK_Programacion_Gestion");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.IdProvincia).HasName("PK__Provinci__66C18BFDC1115E8B");

            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            entity.Property(e => e.IdDepartamento).HasColumnName("id_departamento");
            entity.Property(e => e.NombreProvincia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_provincia");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Provincia__id_de__4E88ABD4");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__6ABCB5E049A19B03");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Ruta>(entity =>
        {
            entity.HasKey(e => e.IdRuta).HasName("PK__Ruta__33C9344FDCE712BF");

            entity.Property(e => e.IdRuta).HasColumnName("id_ruta");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Ruta)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Ruta__id_estado__45F365D3");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__4E3E04AD958D76E5");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.UserUsuario, "UQ__Usuario__2101357C60B6A85C").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.ApellidomUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidom_usuario");
            entity.Property(e => e.ApellidopUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidop_usuario");
            entity.Property(e => e.FechNacimiento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fech_nacimiento");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.NrodniUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nrodni_usuario");
            entity.Property(e => e.PasswordUsuario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_usuario");
            entity.Property(e => e.UserUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_usuario");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Usuario__id_esta__3C69FB99");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuario__id_rol__3D5E1FD2");
        });

        modelBuilder.Entity<Zona>(entity =>
        {
            entity.HasKey(e => e.IdZona).HasName("PK__Zona__67C93611B02E5697");

            entity.ToTable("Zona");

            entity.Property(e => e.IdZona).HasColumnName("id_zona");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Zonas)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Zona__id_estado__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
