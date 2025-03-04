using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string?   LicensePlate { get; set; }
        public int TruckCompany_Id { get; set; }
        public TruckCompany TruckCompany { get; set; }
    }
}