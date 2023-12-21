using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RumahMakanOnline_API.Models
{
    public partial class RestaurantKakiTujuhContext : DbContext
    {
        public RestaurantKakiTujuhContext()
        {
        }

        public RestaurantKakiTujuhContext(DbContextOptions<RestaurantKakiTujuhContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<FoodCategory> FoodCategories { get; set; } = null!;
        public virtual DbSet<FoodProduct> FoodProducts { get; set; } = null!;
        public virtual DbSet<FoodVariant> FoodVariants { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<MenuAccess> MenuAccesses { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderHeader> OrderHeaders { get; set; } = null!;
        public virtual DbSet<TblRole> TblRoles { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-68KSAO6;Initial Catalog=RestaurantKakiTujuh;user id=sa;Password=adadech123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.ContactNum)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("Contact_num");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<FoodCategory>(entity =>
            {
                entity.ToTable("FoodCategory");

                entity.Property(e => e.CatName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("text");
            });

            modelBuilder.Entity<FoodProduct>(entity =>
            {
                entity.ToTable("FoodProduct");

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.NameProduct)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<FoodVariant>(entity =>
            {
                entity.ToTable("FoodVariant");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.VarName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.MenuAction)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.MenuController)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.MenuIcon)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MenuAccess>(entity =>
            {
                entity.ToTable("MenuAccess");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<OrderHeader>(entity =>
            {
                entity.ToTable("OrderHeader");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CodeTransaction).HasMaxLength(20);

                entity.Property(e => e.CustId).HasColumnName("CustID");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("TblRole");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("TblUser");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
