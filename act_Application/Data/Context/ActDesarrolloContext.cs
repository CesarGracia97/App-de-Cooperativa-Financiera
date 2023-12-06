using act_Application.Models.BD;
using act_Application.Models.Sistema.Complementos;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace act_Application.Data.Context;

public partial class ActDesarrolloContext : DbContext
{
    public ActDesarrolloContext()
    {
    }

    public ActDesarrolloContext(DbContextOptions<ActDesarrolloContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActAportacione> ActAportaciones { get; set; }

    public virtual DbSet<ActCapturasPantalla> ActCapturasPantallas { get; set; }

    public virtual DbSet<ActCuentaDestino> ActCuentaDestinos { get; set; }

    public virtual DbSet<ActCuota> ActCuotas { get; set; }

    public virtual DbSet<ActEvento> ActEventos { get; set; }

    public virtual DbSet<ActMulta> ActMultas { get; set; }

    public virtual DbSet<ActNotificacione> ActNotificaciones { get; set; }

    public virtual DbSet<ActPrestamo> ActPrestamos { get; set; }

    public virtual DbSet<ActRol> ActRols { get; set; }

    public virtual DbSet<ActRolUser> ActRolUsers { get; set; }

    public virtual DbSet<ActTablaInteres> ActTablaInteres { get; set; }

    public virtual DbSet<ActUser> ActUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ActAportacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Aportaciones", tb => tb.HasComment("Tabla de Aportaciones"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();
            entity.HasIndex(e => e.IdApor, "IdApor_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdApor, "fk_Aportaciones_Notificaciones");
            entity.HasIndex(e => e.IdUser, "fk_Aportaciones_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdApor).HasMaxLength(45);
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.FechaAportacion).HasColumnType("date");
            entity.Property(e => e.Cuadrante).HasMaxLength(45);
            entity.Property(e => e.Valor).HasPrecision(10);
            entity.Property(e => e.NBancoOrigen).HasMaxLength(45);
            entity.Property(e => e.CBancoOrigen).HasMaxLength(45);
            entity.Property(e => e.NBancoDestino).HasMaxLength(45);
            entity.Property(e => e.CBancoDestino).HasMaxLength(45);
            entity.Property(e => e.Estado).HasMaxLength(45);

        });

        modelBuilder.Entity<ActCapturasPantalla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_CapturasPantallas", tb => tb.HasComment("Capturas de pantalla"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.Origen).HasMaxLength(45);
            entity.Property(e => e.IdOrigenCaptura).HasColumnType("int(11)");
        });

        modelBuilder.Entity<ActCuentaDestino>(entity => 
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_CuentasDestino", tb => tb.HasComment("Tabla de CB"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.NumeroCuentaB).HasMaxLength(45);
            entity.Property(e => e.NombreBanco).HasMaxLength(45);
        });

        modelBuilder.Entity<ActCuota>(entity => 
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Cuotas", tb => tb.HasComment("Tabla de Cuotas"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();
            entity.HasIndex(e => e.IdCuot, "IdCuot_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_Cuotas_User");
            entity.HasIndex(e => e.IdCuot, "fk_Cuotas_Notificaciones");
            entity.HasIndex(e => e.IdPrestamo, "fk_Cuotas_Prestamos");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdCuot).HasMaxLength(45);
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.IdPrestamo).HasColumnType("int(11)");
            entity.Property(e => e.FechaGeneracion).HasColumnType("date");
            entity.Property(e => e.FechaCuota).HasColumnType("date");
            entity.Property(e => e.Valor).HasPrecision(10);
            entity.Property(e => e.Estado).HasMaxLength(45);
            entity.Property(e => e.FechaPago).HasMaxLength(500);
            entity.Property(e => e.CBancoOrigen).HasMaxLength(500);
            entity.Property(e => e.NBancoOrigen).HasMaxLength(500);
            entity.Property(e => e.CBancoDestino).HasMaxLength(500);
            entity.Property(e => e.NBancoDestino).HasMaxLength(500);
            entity.Property(e => e.HistorialValores).HasMaxLength(500);
            entity.Property(e => e.CapturaPantalla).HasMaxLength(500);
        });

        modelBuilder.Entity<ActEvento>(entity => 
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Eventos", tb => tb.HasComment("Tabla de Eventos"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();
            entity.HasIndex(e => e.IdEven, "IdEven_UNIQUE").IsUnique();
            entity.HasIndex(e => e.IdPrestamo, "IdPrestamo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_Eventos_User");
            entity.HasIndex(e => e.IdEven, "fk_Eventos_Notificaciones");
            entity.HasIndex(e => e.IdPrestamo, "fk_Eventos_Prestamos");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdEven).HasMaxLength(45);
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.IdPrestamo).HasColumnType("int(11)");
            entity.Property(e => e.ParticipantesId).HasMaxLength(200);
            entity.Property(e => e.NombresPId).HasMaxLength(2000);
            entity.Property(e => e.FechaGeneracion).HasColumnType("date");
            entity.Property(e => e.FechaInicio).HasColumnType("date");
            entity.Property(e => e.FechaFinalizacion).HasColumnType("date");
            entity.Property(e => e.Estado).HasMaxLength(45);
        });

        modelBuilder.Entity<ActMulta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Multas", tb => tb.HasComment("Tabla de Multas"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();
            entity.HasIndex(e => e.IdMult, "IdMult_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_Multas_User");
            entity.HasIndex(e => e.IdMult, "fk_Multas_Notificaciones");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdMult).HasMaxLength(45);
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.FechaGeneracion).HasColumnType("datetime");
            entity.Property(e => e.Cuadrante).HasMaxLength(45);
            entity.Property(e => e.Razon).HasMaxLength(45);
            entity.Property(e => e.Valor).HasPrecision(10);
            entity.Property(e => e.Estado).HasMaxLength(45);
            entity.Property(e => e.FechaPago).HasMaxLength(500);
            entity.Property(e => e.CBancoOrigen).HasMaxLength(500);
            entity.Property(e => e.NBancoOrigen).HasMaxLength(500);
            entity.Property(e => e.CBancoDestino).HasMaxLength(500);
            entity.Property(e => e.NBancoDestino).HasMaxLength(500);
            entity.Property(e => e.HistorialValores).HasMaxLength(500);
            entity.Property(e => e.CapturaPantalla).HasMaxLength(500);
        });

        modelBuilder.Entity<ActNotificacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Notificaciones", tb => tb.HasComment("Tabla de Notificaciones"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();
            entity.HasIndex(e => e.IdActividad, "IdActividad_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_Notificaciones_User");
            entity.HasIndex(e => e.IdActividad, "fk_Notificaciones_NewUser");
            entity.HasIndex(e => e.IdActividad, "fk_Notificaciones_Aportaciones");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.IdActividad).HasMaxLength(45);
            entity.Property(e => e.FechaGeneracion).HasColumnType("date");
            entity.Property(e => e.Razon).HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(5000);
            entity.Property(e => e.Destino).HasMaxLength(45);
            entity.Property(e => e.Visto).HasMaxLength(45);
        });

        modelBuilder.Entity<ActPrestamo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Prestamos", tb => tb.HasComment("Tabla de presatmos"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();
            entity.HasIndex(e => e.IdPres, "IdPres_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_Prestamos_User");
            entity.HasIndex(e => e.IdPres, "fk_Prestamos_Notificaciones");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.IdPres).HasMaxLength(45);
            entity.Property(e => e.Valor).HasPrecision(10);
            entity.Property(e => e.FechaGeneracion).HasColumnType("date");
            entity.Property(e => e.FechaEntregaDinero).HasColumnType("date");
            entity.Property(e => e.FechaInicioPagoCuotas).HasColumnType("date");
            entity.Property(e => e.FechaPagoTotalPrestamo).HasColumnType("date");
            entity.Property(e => e.TipoCuota).HasMaxLength(45);
            entity.Property(e => e.Estado).HasMaxLength(45);
        });

        modelBuilder.Entity<ActRol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Rol", tb => tb.HasComment("Tabla de Roles"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.NombreRol, "NombreRol_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.DescripcionRol).HasMaxLength(45);
            entity.Property(e => e.NombreRol).HasMaxLength(45);
        });

        modelBuilder.Entity<ActRolUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_RolUser", tb => tb.HasComment("Tabla Relacion Rol Usuario"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_RolUser_User");
            entity.HasIndex(e => e.IdRol, "fk_Socio_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdRol).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
        });

        modelBuilder.Entity<ActTablaInteres>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_TablaIntereses", tb => tb.HasComment("Tabla de Intereses"));

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.IdPersonalizado).HasMaxLength(45);
            entity.Property(e => e.Porcentaje).HasMaxLength(45);
            entity.Property(e => e.Valor).HasPrecision(10);
            entity.Property(e => e.Estado).HasMaxLength(45);
            entity.Property(e => e.ValorGarante).HasPrecision(10);
            entity.Property(e => e.ValorTodos).HasPrecision(10);
        });

        modelBuilder.Entity<ActUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_User", tb => tb.HasComment("Tabla de Usuarios"));

            entity.HasIndex(e => e.IdSocio, "FK_IdSocio");

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Cedula).HasMaxLength(10);
            entity.Property(e => e.Celular).HasMaxLength(45);
            entity.Property(e => e.Contrasena).HasMaxLength(75);
            entity.Property(e => e.Correo).HasMaxLength(45);
            entity.Property(e => e.Estado).HasMaxLength(45);
            entity.Property(e => e.IdSocio).HasColumnType("int(11)");
            entity.Property(e => e.NombreYapellido)
                .HasMaxLength(45)
                .HasColumnName("NombreYApellido");
            entity.Property(e => e.TipoUser).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
