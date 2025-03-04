using ENN_Cargo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.DataAccess
{
    public class ENN_CargoApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ENN_CargoApplicationDbContext(DbContextOptions<ENN_CargoApplicationDbContext> options) : base(options) { }

        public DbSet<CompanyStock> CompanyStocks { get; set; }
        public DbSet<CompanyStocks_Shipments> CompanyStocks_Shipments { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<TruckCompanies_Shipments> TruckCompanies_Shipments { get; set; }
        public DbSet<TruckCompany> TruckCompanies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanyStocks_Shipments>()
                .HasKey(cs => new { cs.CompanyStock_Id, cs.Shipment_Id });

            modelBuilder.Entity<CompanyStocks_Shipments>()
                .HasOne(cs => cs.CompanyStock)
                .WithMany(c => c.CompanyStocks_Shipments)
                .HasForeignKey(cs => cs.CompanyStock_Id);


            modelBuilder.Entity<CompanyStocks_Shipments>()
                .HasOne(cs => cs.Shipment)
                .WithMany(s => s.CompanyStocks_Shipments)
                .HasForeignKey(cs => cs.Shipment_Id);

            modelBuilder.Entity<TruckCompanies_Shipments>()
                .HasKey(ts => new { ts.TruckCompany_Id, ts.Shipment_Id });

            modelBuilder.Entity<TruckCompanies_Shipments>()
                .HasOne(ts => ts.TruckCompany)
                .WithMany(tc => tc.TruckCompanies_Shipments)
                .HasForeignKey(ts => ts.TruckCompany_Id);

            modelBuilder.Entity<TruckCompanies_Shipments>()
                .HasOne(ts => ts.Shipment)
                .WithMany(s => s.TruckCompanies_Shipments)
                .HasForeignKey(ts => ts.Shipment_Id);

            modelBuilder.Entity<Driver>()
                .HasOne(d => d.TruckCompany)
                .WithMany(tc => tc.Drivers)
                .HasForeignKey(d => d.TruckCompany_Id);


            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.TruckCompany)
                .WithMany(tc => tc.Vehicles)
                .HasForeignKey(v => v.TruckCompany_Id);
            modelBuilder.Entity<Driver>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Driver>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TruckCompany>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<TruckCompany>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CompanyStock>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<CompanyStock>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompanyStock>().HasData(
                new CompanyStock { Id = 1, Name = "Glass Industries" ,Address = "Boyana 56", Town = "Sofia" , Country = "Bulgaria" },
                new CompanyStock { Id = 2, Name = "Metal Industries", Address = "Medne Rudnik 23", Town = "Burgas", Country = "Bulgaria" }
            );

            modelBuilder.Entity<TruckCompany>().HasData(
                new TruckCompany { Id = 1, Name = "Express Logistics", Address = "J.K Vasil Levski 23", Town="Kazanlak",Country="Bulgaria" },
                new TruckCompany { Id = 2, Name = "Fast Freight", Address = "Stolipinovo 12",Town="Plovdiv",Country="Bulgaria" }
            );

            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { Id = 1, Brand = "Volvo", Model = "FH16", Year = 2019, LicensePlate = "CT1745AM", TruckCompany_Id = 1 },
                new Vehicle { Id = 2, Brand = "Mercedes", Model = "Actros", Year = 2023, LicensePlate = "CA1999PM", TruckCompany_Id = 2 }
            );

            modelBuilder.Entity<Driver>().HasData(
                new Driver { Id = 1, FirstName = "Kondio", LastName = "Kaloqnov", Experience = 5, TruckCompany_Id = 1 },
                new Driver { Id = 2, FirstName = "Jelqzko", LastName = "Ivanov",  Experience = 8, TruckCompany_Id = 2 }
            );

            modelBuilder.Entity<Shipment>().HasData(
                new Shipment { Id = 1, Description = "Electronics", Weight = 24, FromAddress = "Sklad Kappa", FromTown="Kazanlak",FromCountry="Bulgaria", ToAddress = "Kaufland", ToTown = "Plovdiv", ToCountry = "Bulgaria", PickUpDate = DateTime.Now, DeliveryDate = DateTime.Now.AddDays(3), Status = "Available" },
                new Shipment { Id = 2, Description = "Furniture", Weight = 27, FromAddress = "Sklad Videnov",FromTown="Stara Zagora",FromCountry="Bulgaria", ToAddress = "Metro",ToTown="Sofia",ToCountry="Bulgaria", PickUpDate = DateTime.Now, DeliveryDate = DateTime.Now.AddDays(5), Status = "Available" }
            );
        }
    }
}