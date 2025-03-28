﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ENN_Cargo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PendingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Town = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PendingRequest_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyStocks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyStocks_PendingRequests_PendingRequest_Id",
                        column: x => x.PendingRequest_Id,
                        principalTable: "PendingRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TruckCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Town = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PendingRequest_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TruckCompanies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TruckCompanies_PendingRequests_PendingRequest_Id",
                        column: x => x.PendingRequest_Id,
                        principalTable: "PendingRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    TruckCompany_Id = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PendingRequest_Id = table.Column<int>(type: "int", nullable: true),
                    CompanyStockId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Drivers_CompanyStocks_CompanyStockId",
                        column: x => x.CompanyStockId,
                        principalTable: "CompanyStocks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Drivers_PendingRequests_PendingRequest_Id",
                        column: x => x.PendingRequest_Id,
                        principalTable: "PendingRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Drivers_TruckCompanies_TruckCompany_Id",
                        column: x => x.TruckCompany_Id,
                        principalTable: "TruckCompanies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TruckCompany_Id = table.Column<int>(type: "int", nullable: false),
                    PendingRequest_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_PendingRequests_PendingRequest_Id",
                        column: x => x.PendingRequest_Id,
                        principalTable: "PendingRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_TruckCompanies_TruckCompany_Id",
                        column: x => x.TruckCompany_Id,
                        principalTable: "TruckCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    FromAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromTown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToTown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    PendingRequest_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shipments_PendingRequests_PendingRequest_Id",
                        column: x => x.PendingRequest_Id,
                        principalTable: "PendingRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shipments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyStocks_Shipments",
                columns: table => new
                {
                    CompanyStock_Id = table.Column<int>(type: "int", nullable: false),
                    Shipment_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStocks_Shipments", x => new { x.CompanyStock_Id, x.Shipment_Id });
                    table.ForeignKey(
                        name: "FK_CompanyStocks_Shipments_CompanyStocks_CompanyStock_Id",
                        column: x => x.CompanyStock_Id,
                        principalTable: "CompanyStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyStocks_Shipments_Shipments_Shipment_Id",
                        column: x => x.Shipment_Id,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TruckCompanies_Shipments",
                columns: table => new
                {
                    TruckCompany_Id = table.Column<int>(type: "int", nullable: false),
                    Shipment_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckCompanies_Shipments", x => new { x.TruckCompany_Id, x.Shipment_Id });
                    table.ForeignKey(
                        name: "FK_TruckCompanies_Shipments_Shipments_Shipment_Id",
                        column: x => x.Shipment_Id,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TruckCompanies_Shipments_TruckCompanies_TruckCompany_Id",
                        column: x => x.TruckCompany_Id,
                        principalTable: "TruckCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CompanyStocks",
                columns: new[] { "Id", "Address", "Country", "Name", "PendingRequest_Id", "Town", "UserId" },
                values: new object[,]
                {
                    { 1, "Boyana 56", "Bulgaria", "Glass Industries", null, "Sofia", null },
                    { 2, "Medne Rudnik 23", "Bulgaria", "Metal Industries", null, "Burgas", null }
                });

            migrationBuilder.InsertData(
                table: "Shipments",
                columns: new[] { "Id", "DeliveryDate", "Description", "DriverId", "FromAddress", "FromCountry", "FromTown", "PendingRequest_Id", "PickUpDate", "Status", "ToAddress", "ToCountry", "ToTown", "VehicleId", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 27, 8, 44, 43, 535, DateTimeKind.Local).AddTicks(1875), "Electronics", null, "Sklad Kappa", "Bulgaria", "Kazanlak", null, new DateTime(2025, 3, 24, 8, 44, 43, 535, DateTimeKind.Local).AddTicks(1817), "Available", "Kaufland", "Bulgaria", "Plovdiv", null, 24.0 },
                    { 2, new DateTime(2025, 3, 29, 8, 44, 43, 535, DateTimeKind.Local).AddTicks(1883), "Furniture", null, "Sklad Videnov", "Bulgaria", "Stara Zagora", null, new DateTime(2025, 3, 24, 8, 44, 43, 535, DateTimeKind.Local).AddTicks(1882), "Available", "Metro", "Bulgaria", "Sofia", null, 27.0 }
                });

            migrationBuilder.InsertData(
                table: "TruckCompanies",
                columns: new[] { "Id", "Address", "Country", "Name", "PendingRequest_Id", "Town", "UserId" },
                values: new object[,]
                {
                    { 1, "J.K Vasil Levski 23", "Bulgaria", "Express Logistics", null, "Kazanlak", null },
                    { 2, "Stolipinovo 12", "Bulgaria", "Fast Freight", null, "Plovdiv", null }
                });

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "Id", "CompanyStockId", "Experience", "FirstName", "LastName", "PendingRequest_Id", "TruckCompany_Id", "UserId" },
                values: new object[,]
                {
                    { 1, null, 5, "Kondio", "Kaloqnov", null, 1, null },
                    { 2, null, 8, "Jelqzko", "Ivanov", null, 2, null }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Brand", "LicensePlate", "Model", "PendingRequest_Id", "TruckCompany_Id", "Year" },
                values: new object[,]
                {
                    { 1, "Volvo", "CT1745AM", "FH16", null, 1, 2019 },
                    { 2, "Mercedes", "CA1999PM", "Actros", null, 2, 2023 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStocks_PendingRequest_Id",
                table: "CompanyStocks",
                column: "PendingRequest_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStocks_UserId",
                table: "CompanyStocks",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStocks_Shipments_Shipment_Id",
                table: "CompanyStocks_Shipments",
                column: "Shipment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CompanyStockId",
                table: "Drivers",
                column: "CompanyStockId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_PendingRequest_Id",
                table: "Drivers",
                column: "PendingRequest_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_TruckCompany_Id",
                table: "Drivers",
                column: "TruckCompany_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_UserId",
                table: "Drivers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DriverId",
                table: "Shipments",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_PendingRequest_Id",
                table: "Shipments",
                column: "PendingRequest_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_VehicleId",
                table: "Shipments",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TruckCompanies_PendingRequest_Id",
                table: "TruckCompanies",
                column: "PendingRequest_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TruckCompanies_UserId",
                table: "TruckCompanies",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TruckCompanies_Shipments_Shipment_Id",
                table: "TruckCompanies_Shipments",
                column: "Shipment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PendingRequest_Id",
                table: "Vehicles",
                column: "PendingRequest_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_TruckCompany_Id",
                table: "Vehicles",
                column: "TruckCompany_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CompanyStocks_Shipments");

            migrationBuilder.DropTable(
                name: "TruckCompanies_Shipments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "CompanyStocks");

            migrationBuilder.DropTable(
                name: "TruckCompanies");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PendingRequests");
        }
    }
}
