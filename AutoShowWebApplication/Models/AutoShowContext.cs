using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

#nullable disable

namespace AutoShowWebApplication
{
    public partial class AutoShowContext : DbContext
    {
        public AutoShowContext()
        {
        }

        public AutoShowContext(DbContextOptions<AutoShowContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BodyType> BodyTypes { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Drife> Drives { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= WIN-VCDRI1UG58U; Database=AutoShow; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Ukrainian_CI_AS");

            modelBuilder.Entity<BodyType>(entity =>
            {
                entity.Property(e => e.BodyTypeId).HasColumnName("BodyTypeID");

                entity.Property(e => e.BodyTypeNames)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.BodyTypeId).HasColumnName("BodyTypeID");

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.DriveId).HasColumnName("DriveID");

                entity.Property(e => e.GraduationYear).HasColumnType("int");

                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.Price).HasColumnType("decimal");

                entity.Property(e => e.Image).HasColumnType("varbinary(MAX)");
                entity.Property(e => e.Description).HasColumnType("nvarchar(MAX)");

                entity.HasOne(d => d.BodyType)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.BodyTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Cars_BodyTypes");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Cars_Colors");

                entity.HasOne(d => d.Drive)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.DriveId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Cars_Drive");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Cars_Models");

            })
                ;

            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.ColorName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Telephone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Drife>(entity =>
            {
                entity.HasKey(e => e.DriveId);

                entity.Property(e => e.DriveId).HasColumnName("DriveID");

                entity.Property(e => e.DriveType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Manager>(entity =>
            {
                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Telephone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ModelId).HasColumnName("CarID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.OrderTypeId)
                    //.ValueGeneratedOnAdd()
                    .HasColumnName("OrderTypeID");

                

            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.Property(e => e.OrderTypeId).HasColumnName("OrderTypeID");

                entity.Property(e => e.OrderName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.TypeOfService)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
