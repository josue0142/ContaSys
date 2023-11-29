using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContaSys.Models
{
    public partial class ContaSysContext : DbContext
    {
        public ContaSysContext()
        {
        }

        public ContaSysContext(DbContextOptions<ContaSysContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Auxiliares> Auxiliares { get; set; } = null!;
        public virtual DbSet<CuentaContable> CuentaContables { get; set; } = null!;
        public virtual DbSet<Moneda> Moneda { get; set; } = null!;
        public virtual DbSet<TipoCuenta> TipoCuenta { get; set; } = null!;
        public virtual DbSet<AsientoContable> AsientoContables { get; set; } = null!;
        public virtual DbSet<DetalleAsientoContable> DetalleAsientoContables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=ContaSys;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auxiliares>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CuentaContable>(entity =>
            {
                entity.ToTable("CuentaContable");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PermiteMov)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.CuentaMayor)
                    .WithMany(p => p.InverseCuentaMayor)
                    .HasForeignKey(d => d.CuentaMayorId)
                    .HasConstraintName("FK__CuentaCon__Cuent__2B3F6F97");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.CuentaContables)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("FK__CuentaCon__TipoI__2A4B4B5E");
            });

            modelBuilder.Entity<Moneda>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CodigoIso)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CodigoISO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TasaCambio).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<TipoCuenta>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Origen)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AsientoContable>(entity =>
            {
                entity.ToTable("AsientoContable");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Fecha).HasColumnType("datetime");
              
            });

            modelBuilder.Entity<DetalleAsientoContable>(entity =>
            {
                entity.ToTable("DetalleAsientoContable");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.AsientoContable)
                    .WithMany(p => p.DetalleAsientoContables)
                    .HasForeignKey(d => d.AsientoContableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleAsientoContable_AsientoContable");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
