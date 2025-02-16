using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENN_Cargo.Models
{
    public class CompanyStockViewModel
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string SelectedCountry { get; set; }
        [Required]
        public string SelectedTown { get; set; }
        public List<SelectListItem>? CountryList { get; set; }
        public List<SelectListItem>? TownList { get; set; }
    }
}