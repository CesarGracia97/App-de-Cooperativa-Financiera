﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace act_Application.Models;

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

    public virtual DbSet<ActMultum> ActMulta { get; set; }

    public virtual DbSet<ActRol> ActRols { get; set; }

    public virtual DbSet<ActRolUser> ActRolUsers { get; set; }

    public virtual DbSet<ActTransaccione> ActTransacciones { get; set; }

    public virtual DbSet<ActUser> ActUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=192.168.21.193; port=3306; database=desarrollo; uid=cgarcia; password=cgarcia;");

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
            entity.Property(e => e.CapturaPantalla).HasColumnType("blob");
            entity.Property(e => e.FechaAportacion).HasColumnType("date");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
        });

        modelBuilder.Entity<ActMultum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Multa");

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
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

        modelBuilder.Entity<ActTransaccione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_Transacciones", tb => tb.HasComment("Movimientos"));

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUser, "fk_Transacciones_User");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdUser).HasColumnType("int(11)");
            entity.Property(e => e.Razon).HasMaxLength(45);
        });

        modelBuilder.Entity<ActUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("act_User", tb => tb.HasComment("Tabla de Usuarios"));

            entity.HasIndex(e => e.Cbancaria, "CBancaria_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Cedula, "Cedula_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Correo, "Correo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Cbancaria)
                .HasColumnType("int(11)")
                .HasColumnName("CBancaria");
            entity.Property(e => e.Cedula).HasColumnType("int(11)");
            entity.Property(e => e.Celular).HasMaxLength(45);
            entity.Property(e => e.Contrasena).HasMaxLength(75);
            entity.Property(e => e.Correo).HasMaxLength(45);
            entity.Property(e => e.Ncaccionario)
                .HasColumnType("int(11)")
                .HasColumnName("NCAccionario");
            entity.Property(e => e.NombreYapellido)
                .HasMaxLength(45)
                .HasColumnName("NombreYApellido");
            entity.Property(e => e.TipoUser).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
