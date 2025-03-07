using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENN_Cargo.Models
{
    public class ShipmentViewModel
    {
        public int? Id { get; set; }         
        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Weight is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        public double Weight { get; set; }
        public double? MinWeight { get; set; }
        public double? MaxWeight { get; set; }
        [Required(ErrorMessage = "Origin country is required.")]
        public string? SelectedFromCountry { get; set; }
        [Required(ErrorMessage = "Origin town is required.")]
        public string? SelectedFromTown { get; set; }
        [Required(ErrorMessage = "From address is required.")]
        public string? FromAddress { get; set; }
        [Required(ErrorMessage = "Destination country is required.")]
        public string? SelectedToCountry { get; set; }
        [Required(ErrorMessage = "Destination town is required.")]
        public string? SelectedToTown { get; set; }
        [Required(ErrorMessage = "To address is required.")]
        public string? ToAddress { get; set; }
        [Required(ErrorMessage = "Pickup date is required.")]
        public DateTime? PickUpDate { get; set; }
        [Required(ErrorMessage = "Delivery date is required.")]
        public DateTime? DeliveryDate { get; set; }
        public SelectList? FromCountries { get; set; }
        public SelectList? FromTowns { get; set; }
        public SelectList? ToCountries { get; set; }
        public SelectList? ToTowns { get; set; }
        public List<Shipment>? Shipments { get; set; } = new List<Shipment>();
        public Shipment? Shipment { get; set; } = new Shipment();
        public int? SelectedCompanyStockId { get; set; }
        public SelectList? CompanyStocks { get; set; }
        public Dictionary<string, List<string>>? PredefinedTowns { get; set; } = new Dictionary<string, List<string>>();
        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }
    }
}