using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;namespace ENN_Cargo.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string FromAddress { get; set; }
        public string FromCountry { get; set; }
        public string FromTown { get; set; }
        public string ToAddress { get; set; }
        public string ToCountry { get; set; }
        public string ToTown { get; set; }
        public DateTime? PickUpDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string Status { get; set; }
        public int? DriverId { get; set; }         public Driver Driver { get; set; }         public int? VehicleId { get; set; }         public Vehicle Vehicle { get; set; }
        public ICollection<CompanyStocks_Shipments> CompanyStocks_Shipments { get; set; }
        public ICollection<TruckCompanies_Shipments> TruckCompanies_Shipments { get; set; }
    }
}