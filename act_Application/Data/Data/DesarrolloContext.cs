using System;
using System.Collections.Generic;
using act_Application.Models.BD;
using Microsoft.EntityFrameworkCore;

namespace act_Application.Models.BD;

public partial class DesarrolloContext : DbContext
{
    public DesarrolloContext()
    {
    }

    public DesarrolloContext(DbContextOptions<DesarrolloContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActAportacione> ActAportaciones { get; set; }

    public virtual DbSet<ActAportacionesAc> ActAportacionesAcs { get; set; }

    public virtual DbSet<ActCuentaDestino> ActCuentaDestinos { get; set; }

    public virtual DbSet<ActCuota> ActCuotas { get; set; }

    public virtual DbSet<ActEvento> ActEventos { get; set; }

    public virtual DbSet<ActLapmt> ActLapmts { get; set; }

    public virtual DbSet<ActMulta> ActMultas { get; set; }

    public virtual DbSet<ActMultasAc> ActMultasAcs { get; set; }

    public virtual DbSet<ActNotificacione> ActNotificaciones { get; set; }

    public virtual DbSet<ActPorcentaje> ActPorcentajes { get; set; }

    public virtual DbSet<ActPrestamo> ActPrestamos { get; set; }

    public virtual DbSet<ActQuery> ActQuerys { get; set; }

    public virtual DbSet<ActRol> ActRols { get; set; }

    public virtual DbSet<ActRolUser> ActRolUsers { get; set; }

    public virtual DbSet<ActUser> ActUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActAportacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Aportaciones", tb => tb.HasComment("Aportaciones ec"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_Aportaciones_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Aprobacion).HasMaxLength(10);
            entity.Property(e => e.BancoDestino).HasMaxLength(45);
            entity.Property(e => e.Cbancaria)
                .HasMaxLength(15)
                .HasColumnName("CBancaria");
            entity.Property(e => e.Cuadrante1).HasColumnType("int(1)");
            entity.Property(e => e.Cuadrante2).HasColumnType("int(1)");
            entity.Property(e => e.CuentaDestino).HasMaxLength(12);
            entity.Property(e => e.FechaAportacion).HasColumnType("date");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.Nbanco)
                .HasMaxLength(45)
                .HasColumnName("NBanco");
            entity.Property(e => e.Razon).HasMaxLength(45);
            entity.Property(e => e.Valor).HasPrecision(10);
        });

        modelBuilder.Entity<ActAportacionesAc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_AportacionesAC", tb => tb.HasComment("Tabla de Liquidacion de Aportes. AportacionesAC"));

            entity.HasIndex(e => e.IdUser, "IdUser_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.AportacionesIds).HasMaxLength(200);
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.LiquidacionTotal).HasPrecision(10);
            entity.Property(e => e.ValorAc)
                .HasPrecision(10)
                .HasColumnName("ValorAC");
        });

        modelBuilder.Entity<ActCuentaDestino>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_CuentaDestino", tb => tb.HasComment("Cuentas Bancarias de Destino"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.NumeroCuenta, "NumeroCuenta_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Detalles).HasMaxLength(900);
            entity.Property(e => e.DuenoCuenta).HasMaxLength(45);
            entity.Property(e => e.NombreBanco).HasMaxLength(45);
            entity.Property(e => e.NumeroCuenta).HasMaxLength(12);
        });

        modelBuilder.Entity<ActCuota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Cuotas", tb => tb.HasComment("Tabla de Cuotas, aqui se almacena el Id del Usuario, el Id d"));

            entity.HasIndex(e => e.IdTransaccion, "fk_Cuotas_Transacciones");

            entity.HasIndex(e => e.IdUser, "fk_Cuotas_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Estado).HasMaxLength(45);
            entity.Property(e => e.FechaCuota).HasColumnType("datetime");
            entity.Property(e => e.IdTransaccion).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.ValorCuota).HasPrecision(10);
        });

        modelBuilder.Entity<ActEvento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Eventos", tb => tb.HasComment("Tabla de Participantes/Garantes de Prestamo"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdPrestamo, "fk_Participantes_Transacciones");

            entity.HasIndex(e => e.IdUser, "fk_Participantes_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Estado).HasMaxLength(45);
            entity.Property(e => e.FechaFinalizacion).HasColumnType("date");
            entity.Property(e => e.FechaGeneracion).HasColumnType("date");
            entity.Property(e => e.FechaInicio).HasColumnType("date");
            entity.Property(e => e.IdPrestamo).HasColumnType("int(11)");
            entity.Property(e => e.IdUser)
                .HasComment("Esta Columna es la kue identifica el dueño de la transaccion con la Participacion, y se relaciona con la tabla usuario.")
                .HasColumnType("int(11)");
            entity.Property(e => e.ParticipantesId).HasMaxLength(100);
            entity.Property(e => e.ParticipantesNombre).HasMaxLength(10000);
        });

        modelBuilder.Entity<ActLaptm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_LAPTM", tb => tb.HasComment("Tabla Relacion:Liquidacion,Aportes,Prestamos,Multas,Usuario"));

            entity.HasIndex(e => e.IdAportacionesAc, "IdAportacionesAC_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdMultasAc, "IdMultasAC_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdPrestamosAc, "IdPrestamosAC_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "IdUser_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdAportacionesAc)
                .HasColumnType("int(11)")
                .HasColumnName("IdAportacionesAC");
            entity.Property(e => e.IdMultasAc)
                .HasColumnType("int(11)")
                .HasColumnName("IdMultasAC");
            entity.Property(e => e.IdPrestamosAc)
                .HasColumnType("int(11)")
                .HasColumnName("IdPrestamosAC");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
        });

        modelBuilder.Entity<ActMulta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Multas", tb => tb.HasComment("Tabla de Multas"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdAportacion, "fk_Multas_Aportaciones");

            entity.HasIndex(e => e.IdUser, "fk_Multas_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Aprobacion).HasMaxLength(45);
            entity.Property(e => e.Cuadrante1).HasColumnType("int(1)");
            entity.Property(e => e.Cuadrante2).HasColumnType("int(1)");
            entity.Property(e => e.FechaMulta).HasColumnType("date");
            entity.Property(e => e.IdAportacion).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.Porcentaje).HasPrecision(10);
            entity.Property(e => e.Valor).HasPrecision(10);
        });

        modelBuilder.Entity<ActMultasAc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_MultasAC", tb => tb.HasComment("Tabla de Liquidacion de Multas. MultasAC"));

            entity.HasIndex(e => e.IdUser, "IdUser_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.LiquidacionTotal).HasPrecision(10);
            entity.Property(e => e.MultasIds).HasMaxLength(200);
            entity.Property(e => e.ValorAc)
                .HasPrecision(10)
                .HasColumnName("ValorAC");
        });

        modelBuilder.Entity<ActNotificacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Notificaciones", tb => tb.HasComment("Tabla de Notificaciones"));

            entity.HasIndex(e => e.IdAportaciones, "fk_Notificaciones_Aportaciones");

            entity.HasIndex(e => e.IdCuotas, "fk_Notificaciones_Cuotas");

            entity.HasIndex(e => e.IdPrestamo, "fk_Notificaciones_Transacciones");

            entity.HasIndex(e => e.IdUser, "fk_Notificaciones_User");

            entity.HasIndex(e => e.Id, "idact_Notificaciones_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Descripcion).HasColumnType("mediumtext");
            entity.Property(e => e.Destino).HasMaxLength(13);
            entity.Property(e => e.FechaNotificacion).HasColumnType("datetime");
            entity.Property(e => e.IdAportaciones).HasColumnType("int(11)");
            entity.Property(e => e.IdCuotas).HasColumnType("int(11)");
            entity.Property(e => e.IdPrestamo).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.Razon).HasMaxLength(90);
            entity.Property(e => e.Visto).HasColumnType("int(1)");
        });

        modelBuilder.Entity<ActPorcentaje>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Porcentaje");

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Categoria).HasMaxLength(45);
            entity.Property(e => e.Condicion).HasMaxLength(200);
            entity.Property(e => e.Porcentaje).HasPrecision(10);
            entity.Property(e => e.Razon).HasMaxLength(45);
            entity.Property(e => e.Target).HasMaxLength(45);
        });

        modelBuilder.Entity<ActPrestamo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Prestamos", tb => tb.HasComment("Operaciones de Referentes"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdParticipantes, "fk_Transacciones_Participantes");

            entity.HasIndex(e => e.IdUser, "fk_Transacciones_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Estado).HasMaxLength(45);
            entity.Property(e => e.FechaEntregaDinero).HasColumnType("date");
            entity.Property(e => e.FechaGeneracion).HasColumnType("date");
            entity.Property(e => e.FechaIniCoutaPrestamo).HasColumnType("date");
            entity.Property(e => e.FechaPagoTotalPrestamo).HasColumnType("date");
            entity.Property(e => e.IdParticipantes).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.Razon).HasMaxLength(45);
            entity.Property(e => e.TipoCuota).HasMaxLength(45);
            entity.Property(e => e.Valor).HasPrecision(10);
        });

        modelBuilder.Entity<ActQuery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Querys", tb => tb.HasComment("Querys para consultas"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Descripcion).HasMaxLength(5000);
            entity.Property(e => e.NombreQuery).HasMaxLength(60);
            entity.Property(e => e.Query).HasMaxLength(2000);
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

            entity.HasIndex(e => e.IdRol, "fk_RolUser_Rol");

            entity.HasIndex(e => e.IdUser, "fk_RolUser_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdRol).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
        });

        modelBuilder.Entity<ActUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_User", tb => tb.HasComment("Tabla de Usuarios"));

            entity.HasIndex(e => e.Cedula, "Cedula_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Correo, "Correo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdSocio, "fk_UserSocio_UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Activo).HasColumnType("int(1)");
            entity.Property(e => e.Cedula).HasMaxLength(10);
            entity.Property(e => e.Celular).HasMaxLength(45);
            entity.Property(e => e.Contrasena).HasMaxLength(75);
            entity.Property(e => e.Correo).HasMaxLength(45);
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
