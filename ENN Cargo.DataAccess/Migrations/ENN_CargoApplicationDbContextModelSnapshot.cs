﻿using System;
using ENN_Cargo.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;#nullable disablenamespace ENN_Cargo.DataAccess.Migrations
{
    [DbContext(typeof(ENN_CargoApplicationDbContext))]
    partial class ENN_CargoApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);            modelBuilder.Entity("ENN_Cargo.Models.CompanyStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Town")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");                    b.HasKey("Id");                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");                    b.ToTable("CompanyStocks");                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Boyana 56",
                            Country = "Bulgaria",
                            Name = "Glass Industries",
                            Town = "Sofia"
                        },
                        new
                        {
                            Id = 2,
                            Address = "Medne Rudnik 23",
                            Country = "Bulgaria",
                            Name = "Metal Industries",
                            Town = "Burgas"
                        });
                });            modelBuilder.Entity("ENN_Cargo.Models.CompanyStocks_Shipments", b =>
                {
                    b.Property<int>("CompanyStock_Id")
                        .HasColumnType("int");                    b.Property<int>("Shipment_Id")
                        .HasColumnType("int");                    b.HasKey("CompanyStock_Id", "Shipment_Id");                    b.HasIndex("Shipment_Id");                    b.ToTable("CompanyStocks_Shipments");
                });            modelBuilder.Entity("ENN_Cargo.Models.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));                    b.Property<int?>("CompanyStockId")
                        .HasColumnType("int");                    b.Property<int>("Experience")
                        .HasColumnType("int");                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<int?>("TruckCompany_Id")
                        .HasColumnType("int");                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");                    b.HasKey("Id");                    b.HasIndex("CompanyStockId");                    b.HasIndex("TruckCompany_Id");                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");                    b.ToTable("Drivers");                    b.HasData(
                        new
                        {
                            Id = 1,
                            Experience = 5,
                            FirstName = "Kondio",
                            LastName = "Kaloqnov",
                            TruckCompany_Id = 1
                        },
                        new
                        {
                            Id = 2,
                            Experience = 8,
                            FirstName = "Jelqzko",
                            LastName = "Ivanov",
                            TruckCompany_Id = 2
                        });
                });            modelBuilder.Entity("ENN_Cargo.Models.Shipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("datetime2");                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<int?>("DriverId")
                        .HasColumnType("int");                    b.Property<string>("FromAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("FromCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("FromTown")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<DateTime?>("PickUpDate")
                        .HasColumnType("datetime2");                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("ToAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("ToCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("ToTown")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<int?>("VehicleId")
                        .HasColumnType("int");                    b.Property<double>("Weight")
                        .HasColumnType("float");                    b.HasKey("Id");                    b.HasIndex("DriverId");                    b.HasIndex("VehicleId");                    b.ToTable("Shipments");                    b.HasData(
                        new
                        {
                            Id = 1,
                            DeliveryDate = new DateTime(2025, 3, 10, 16, 0, 5, 727, DateTimeKind.Local).AddTicks(7847),
                            Description = "Electronics",
                            FromAddress = "Sklad Kappa",
                            FromCountry = "Bulgaria",
                            FromTown = "Kazanlak",
                            PickUpDate = new DateTime(2025, 3, 7, 16, 0, 5, 727, DateTimeKind.Local).AddTicks(7799),
                            Status = "Available",
                            ToAddress = "Kaufland",
                            ToCountry = "Bulgaria",
                            ToTown = "Plovdiv",
                            Weight = 24.0
                        },
                        new
                        {
                            Id = 2,
                            DeliveryDate = new DateTime(2025, 3, 12, 16, 0, 5, 727, DateTimeKind.Local).AddTicks(7855),
                            Description = "Furniture",
                            FromAddress = "Sklad Videnov",
                            FromCountry = "Bulgaria",
                            FromTown = "Stara Zagora",
                            PickUpDate = new DateTime(2025, 3, 7, 16, 0, 5, 727, DateTimeKind.Local).AddTicks(7854),
                            Status = "Available",
                            ToAddress = "Metro",
                            ToCountry = "Bulgaria",
                            ToTown = "Sofia",
                            Weight = 27.0
                        });
                });            modelBuilder.Entity("ENN_Cargo.Models.TruckCompanies_Shipments", b =>
                {
                    b.Property<int>("TruckCompany_Id")
                        .HasColumnType("int");                    b.Property<int>("Shipment_Id")
                        .HasColumnType("int");                    b.HasKey("TruckCompany_Id", "Shipment_Id");                    b.HasIndex("Shipment_Id");                    b.ToTable("TruckCompanies_Shipments");
                });            modelBuilder.Entity("ENN_Cargo.Models.TruckCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Town")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");                    b.HasKey("Id");                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");                    b.ToTable("TruckCompanies");                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "J.K Vasil Levski 23",
                            Country = "Bulgaria",
                            Name = "Express Logistics",
                            Town = "Kazanlak"
                        },
                        new
                        {
                            Id = 2,
                            Address = "Stolipinovo 12",
                            Country = "Bulgaria",
                            Name = "Fast Freight",
                            Town = "Plovdiv"
                        });
                });            modelBuilder.Entity("ENN_Cargo.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("LicensePlate")
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");                    b.Property<int>("TruckCompany_Id")
                        .HasColumnType("int");                    b.Property<int>("Year")
                        .HasColumnType("int");                    b.HasKey("Id");                    b.HasIndex("TruckCompany_Id");                    b.ToTable("Vehicles");                    b.HasData(
                        new
                        {
                            Id = 1,
                            Brand = "Volvo",
                            LicensePlate = "CT1745AM",
                            Model = "FH16",
                            TruckCompany_Id = 1,
                            Year = 2019
                        },
                        new
                        {
                            Id = 2,
                            Brand = "Mercedes",
                            LicensePlate = "CA1999PM",
                            Model = "Actros",
                            TruckCompany_Id = 2,
                            Year = 2023
                        });
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");                    b.HasKey("Id");                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");                    b.ToTable("AspNetRoles", (string)null);
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");                    b.HasKey("Id");                    b.HasIndex("RoleId");                    b.ToTable("AspNetRoleClaims", (string)null);
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");                    b.HasKey("Id");                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");                    b.ToTable("AspNetUsers", (string)null);
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");                    b.HasKey("Id");                    b.HasIndex("UserId");                    b.ToTable("AspNetUserClaims", (string)null);
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");                    b.HasKey("LoginProvider", "ProviderKey");                    b.HasIndex("UserId");                    b.ToTable("AspNetUserLogins", (string)null);
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");                    b.HasKey("UserId", "RoleId");                    b.HasIndex("RoleId");                    b.ToTable("AspNetUserRoles", (string)null);
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");                    b.HasKey("UserId", "LoginProvider", "Name");                    b.ToTable("AspNetUserTokens", (string)null);
                });            modelBuilder.Entity("ENN_Cargo.Models.CompanyStock", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithOne()
                        .HasForeignKey("ENN_Cargo.Models.CompanyStock", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);                    b.Navigation("User");
                });            modelBuilder.Entity("ENN_Cargo.Models.CompanyStocks_Shipments", b =>
                {
                    b.HasOne("ENN_Cargo.Models.CompanyStock", "CompanyStock")
                        .WithMany("CompanyStocks_Shipments")
                        .HasForeignKey("CompanyStock_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();                    b.HasOne("ENN_Cargo.Models.Shipment", "Shipment")
                        .WithMany("CompanyStocks_Shipments")
                        .HasForeignKey("Shipment_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();                    b.Navigation("CompanyStock");                    b.Navigation("Shipment");
                });            modelBuilder.Entity("ENN_Cargo.Models.Driver", b =>
                {
                    b.HasOne("ENN_Cargo.Models.CompanyStock", null)
                        .WithMany("Drivers")
                        .HasForeignKey("CompanyStockId");                    b.HasOne("ENN_Cargo.Models.TruckCompany", "TruckCompany")
                        .WithMany("Drivers")
                        .HasForeignKey("TruckCompany_Id");                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithOne()
                        .HasForeignKey("ENN_Cargo.Models.Driver", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);                    b.Navigation("TruckCompany");                    b.Navigation("User");
                });            modelBuilder.Entity("ENN_Cargo.Models.Shipment", b =>
                {
                    b.HasOne("ENN_Cargo.Models.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId");                    b.HasOne("ENN_Cargo.Models.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId");                    b.Navigation("Driver");                    b.Navigation("Vehicle");
                });            modelBuilder.Entity("ENN_Cargo.Models.TruckCompanies_Shipments", b =>
                {
                    b.HasOne("ENN_Cargo.Models.Shipment", "Shipment")
                        .WithMany("TruckCompanies_Shipments")
                        .HasForeignKey("Shipment_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();                    b.HasOne("ENN_Cargo.Models.TruckCompany", "TruckCompany")
                        .WithMany("TruckCompanies_Shipments")
                        .HasForeignKey("TruckCompany_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();                    b.Navigation("Shipment");                    b.Navigation("TruckCompany");
                });            modelBuilder.Entity("ENN_Cargo.Models.TruckCompany", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithOne()
                        .HasForeignKey("ENN_Cargo.Models.TruckCompany", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);                    b.Navigation("User");
                });            modelBuilder.Entity("ENN_Cargo.Models.Vehicle", b =>
                {
                    b.HasOne("ENN_Cargo.Models.TruckCompany", "TruckCompany")
                        .WithMany("Vehicles")
                        .HasForeignKey("TruckCompany_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();                    b.Navigation("TruckCompany");
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });            modelBuilder.Entity("ENN_Cargo.Models.CompanyStock", b =>
                {
                    b.Navigation("CompanyStocks_Shipments");                    b.Navigation("Drivers");
                });            modelBuilder.Entity("ENN_Cargo.Models.Shipment", b =>
                {
                    b.Navigation("CompanyStocks_Shipments");                    b.Navigation("TruckCompanies_Shipments");
                });            modelBuilder.Entity("ENN_Cargo.Models.TruckCompany", b =>
                {
                    b.Navigation("Drivers");                    b.Navigation("TruckCompanies_Shipments");                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
