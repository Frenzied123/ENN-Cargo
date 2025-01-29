using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.Models
{
    public class TruckCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<TruckCompanies_Shipments> TruckCompanies_Shipments { get; set; }
        public ICollection<Driver> Drivers { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}