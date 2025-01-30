using Microsoft.AspNetCore.Mvc.Rendering;

namespace ENN_Cargo.Models
{
    public class VehicleViewModel
    {
        public string SelectedBrand { get; set; }
        public string SelectedModel { get; set; }
        public int? SelectedYear { get; set; }
        public string SelectedLicensePlateCountry { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Models { get; set; }
        public SelectList Years { get; set; }
        public SelectList LicensePlateCountries { get; set; }
    }
}