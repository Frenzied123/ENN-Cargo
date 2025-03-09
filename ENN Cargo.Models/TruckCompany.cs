using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;namespace ENN_Cargo.Models
{
    public class TruckCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public ICollection<TruckCompanies_Shipments>? TruckCompanies_Shipments { get; set; }
        public ICollection<Driver>? Drivers { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}