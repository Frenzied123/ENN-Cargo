using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;namespace ENN_Cargo.Models
{
    public class TruckCompanyViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string SelectedCountry { get; set; }
        public string SelectedTown { get; set; }
        public string PhoneNumber { get; set; }
        public int? MinDrivers { get; set; }
        public int? MaxDrivers { get; set; }
        public int? MinVehicles { get; set; }
        public int? MaxVehicles { get; set; }
        public SelectList Countries { get; set; }
        public SelectList Towns { get; set; }
        public List<TruckCompanyItem> TruckCompanies { get; set; } = new List<TruckCompanyItem>();
        public class TruckCompanyItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Country { get; set; }
            public string Town { get; set; }
            public string PhoneNumber { get; set; }
            public List<Driver> Drivers { get; set; }
            public List<Vehicle> Vehicles { get; set; }
        }
    }
}