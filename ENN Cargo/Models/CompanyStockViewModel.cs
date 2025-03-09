using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;namespace ENN_Cargo.Models
{
    public class CompanyStockViewModel
    {
        [Key]
        public int? Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public string? SelectedCountry { get; set; }
        public string? SelectedTown { get; set; } 
        public string? CustomTown { get; set; }
        public List<SelectListItem>? CountryList { get; set; }
        public List<SelectListItem>? TownList { get; set; }
    }
}