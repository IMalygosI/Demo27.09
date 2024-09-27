using System;
using System.Collections.Generic;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Context;

public partial class User738Context : DbContext
{
    public User738Context()
    {
    }

    public User738Context(DbContextOptions<User738Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSale> ProductSales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.2.159;Port=5432;Database=user738;Username=user738;password=14053");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_pk");

            entity.ToTable("activity", "Demo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("images_pk");

            entity.ToTable("images", "Demo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.Picture)
                .HasColumnType("character varying")
                .HasColumnName("picture");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Images)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("images_product_fk");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("newtable_pk");

            entity.ToTable("manufacturer", "Demo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ManufacturerName)
                .HasMaxLength(100)
                .HasColumnName("Manufacturer Name");
            entity.Property(e => e.StartDate).HasColumnName("Start date");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pk");

            entity.ToTable("product", "Demo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Cost).HasPrecision(19, 4);
            entity.Property(e => e.Description)
                .HasMaxLength(1)
                .HasColumnName("description");
            entity.Property(e => e.IdActivity).HasColumnName("id_activity");
            entity.Property(e => e.IdManufacturer).HasColumnName("id_manufacturer");
            entity.Property(e => e.MainImagePath)
                .HasMaxLength(1000)
                .HasColumnName("Main_Image_Path");
            entity.Property(e => e.TovarName)
                .HasMaxLength(100)
                .HasColumnName("Tovar_Name");

            entity.HasOne(d => d.IdActivityNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdActivity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_activity_fk");

            entity.HasOne(d => d.IdManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdManufacturer)
                .HasConstraintName("product_manufacturer_fk");
        });

        modelBuilder.Entity<ProductSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tovarlist_pk");

            entity.ToTable("product_sale", "Demo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DateOfSale).HasColumnName("Date of sale");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.SalesTime).HasColumnName("Sales time");
            entity.Property(e => e.TovarName)
                .HasColumnType("character varying")
                .HasColumnName("Tovar Name");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("product_sale_product_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
