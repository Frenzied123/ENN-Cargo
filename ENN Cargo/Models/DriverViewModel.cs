using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;namespace ENN_Cargo.Models
{
    public class DriverViewModel
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Experience { get; set; }
        public string PhoneNumber { get; set; }
        public int? SelectedTruckCompanyId { get; set; }
        public int? MinExperience { get; set; }
        public int? MaxExperience { get; set; }
        public string SortByExperience { get; set; }
        public string SortByTruckCompany { get; set; }
        public SelectList TruckCompanyList { get; set; }
        public SelectList SortByExperienceOptions { get; set; }
        public SelectList SortByTruckCompanyOptions { get; set; }
        public List<DriverItem> Drivers { get; set; } = new List<DriverItem>();
        public class DriverItem
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public int Experience { get; set; }
            public string PhoneNumber { get; set; }
            public TruckCompany TruckCompany { get; set; }
        }
    }
}