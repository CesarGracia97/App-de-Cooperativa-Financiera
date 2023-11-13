using act_Application.Models.BD;
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

    public virtual DbSet<ActPrestamo> ActPrestamos { get; set; }

    public virtual DbSet<ActRol> ActRols { get; set; }

    public virtual DbSet<ActRolUser> ActRolUsers { get; set; }

    public virtual DbSet<ActUser> ActUsers { get; set; }

    public virtual DbSet<ActNotificacione> ActNotificaciones { get; set; }

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
            entity.Property(e => e.Aprobacion).HasMaxLength(45);

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
            entity.HasIndex(e => e.IdEvento, "IdPres_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_Prestamos_User");
            entity.HasIndex(e => e.IdPres, "fk_Prestamos_Notificaciones");
            entity.HasIndex(e => e.IdEvento, "fk_Prestamos_Eventos");

            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.IdPres).HasMaxLength(45);
            entity.Property(e => e.IdEvento).HasColumnType("int(11)");
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
