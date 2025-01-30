using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ENN_Cargo.Models
{
    public class ShipmentFilterViewModel
    {
        public double? MinWeight { get; set; }
        public double? MaxWeight { get; set; }
        public string SelectedFromCountry { get; set; }
        public string SelectedFromTown { get; set; }
        public string SelectedToCountry { get; set; }
        public string SelectedToTown { get; set; }
        public DateTime? PickUpDateFrom { get; set; }
        public DateTime? PickUpDateTo { get; set; }
        public DateTime? DeliveryDateFrom { get; set; }
        public DateTime? DeliveryDateTo { get; set; }
        public SelectList FromCountries { get; set; }
        public SelectList FromTowns { get; set; }
        public SelectList ToCountries { get; set; }
        public SelectList ToTowns { get; set; }
        public List<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}