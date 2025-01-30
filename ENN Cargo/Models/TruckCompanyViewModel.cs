using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ENN_Cargo.Models
{
    public class TruckCompanyViewModel
    {
        public List<TruckCompany> TruckCompanies { get; set; } = new List<TruckCompany>();
        public int? MinDrivers { get; set; }
        public int? MaxDrivers { get; set; }
        public int? MinVehicles { get; set; }
        public int? MaxVehicles { get; set; }
        public string SelectedCountry { get; set; }
        public string SelectedTown { get; set; }
        public SelectList Countries { get; set; } = new SelectList(new List<string>());
        public SelectList Towns { get; set; } = new SelectList(new List<string>());
    }
}
