using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Experience { get; set; }
        public int? TruckCompany_Id { get; set; }
        public TruckCompany? TruckCompany { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}