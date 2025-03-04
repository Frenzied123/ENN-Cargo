using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENN_Cargo.Models
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Brand is required.")]
        public string? Brand { get; set; }
        [Required(ErrorMessage = "Model is required.")]
        public string? Model { get; set; }
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int? Year { get; set; }
        [Required(ErrorMessage = "License Plate is required.")]
        public string? LicensePlate { get; set; }
        [Required(ErrorMessage = "Please select a truck company.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid truck company.")]
        public int? SelectedTruckCompanyId { get; set; }
        public SelectList? TruckCompanies { get; set; }
        public string? SelectedBrand { get; set; }
        public string? SelectedModel { get; set; }
        public int? SelectedYear { get; set; }
        public string? SelectedLicensePlateCountry { get; set; }
        public List<Vehicle>? Vehicles { get; set; }
        public SelectList? Brands { get; set; }
        public SelectList? Models { get; set; }
        public SelectList? Years { get; set; }
        public SelectList? LicensePlateCountries { get; set; }
    }
}