using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ENN_Cargo.Models
{
    public class DriverViewModel
    {
        public List<Driver> Drivers { get; set; }
        public int? MinExperience { get; set; }
        public int? MaxExperience { get; set; }
        public string SortByTruckCompany { get; set; }
        public string SortByExperience { get; set; }
        public SelectList SortingOptions { get; set; }
        public DriverViewModel()
        {
            Drivers = new List<Driver>();
            SortingOptions = new SelectList(new List<string> { "A-Z", "Z-A", "Low-High", "High-Low" });
        }
    }
}