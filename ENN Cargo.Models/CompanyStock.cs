﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ENN_Cargo.Models
{
    public class CompanyStock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Town { get; set; }
        public ICollection<CompanyStocks_Shipments>? CompanyStocks_Shipments { get; set; }
        public ICollection<Driver>? Drivers { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public int? PendingRequest_Id { get; set; }
        public PendingRequest? PendingRequest { get; set; }
    }
}