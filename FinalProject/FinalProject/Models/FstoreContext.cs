using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinalProject.Models;

public partial class FstoreContext : DbContext
{
    public FstoreContext()
    {
    }

    public FstoreContext(DbContextOptions<FstoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<ImportOrder> ImportOrders { get; set; }

    public virtual DbSet<ImportOrderDetail> ImportOrderDetails { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brands__DAD4F3BEDCFC4A34");

            entity.Property(e => e.BrandId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BrandID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BFE3C648E");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedOnAdd()
                .HasColumnName("CategoryID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8F66F6E36");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IsBlock).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1F26F8F1A");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Employees__RoleI__398D8EEE");
        });

        modelBuilder.Entity<ImportOrder>(entity =>
        {
            entity.HasKey(e => e.Ioid).HasName("PK__ImportOr__8E7764F355835EF5");

            entity.Property(e => e.Ioid).HasColumnName("IOID");
            entity.Property(e => e.Completed).HasDefaultValue(false);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.ImportDate).HasColumnType("datetime");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            entity.HasOne(d => d.Employee).WithMany(p => p.ImportOrders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__ImportOrd__Emplo__4D94879B");

            entity.HasOne(d => d.Supplier).WithMany(p => p.ImportOrders)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__ImportOrd__Suppl__4E88ABD4");
        });

        modelBuilder.Entity<ImportOrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.Ioid, e.ProductId }).HasName("PK__ImportOr__4537A89DAA0693D9");

            entity.Property(e => e.Ioid).HasColumnName("IOID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Io).WithMany(p => p.ImportOrderDetails)
                .HasForeignKey(d => d.Ioid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportOrde__IOID__5165187F");

            entity.HasOne(d => d.Product).WithMany(p => p.ImportOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportOrd__Produ__52593CB8");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF084C9EB0");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Address).HasColumnType("ntext");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DeliveredDate).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.OrderedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__5535A963");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__OrderDet__08D097C1EA3561FE");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__5812160E");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Produ__59063A47");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC2769D4B346");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDDD6A68D6");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Products__BrandI__48CFD27E");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__49C3F6B7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A39ADFAA2");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE6669433797237");

            entity.Property(e => e.SupplierId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SupplierID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.IsActivate).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.LastModify).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TaxId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TaxID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
