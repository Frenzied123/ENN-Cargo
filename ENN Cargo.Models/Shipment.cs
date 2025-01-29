using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public DateTime? PickUpDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public ICollection<CompanyStocks_Shipments> CompanyStocks_Shipments { get; set; }
        public ICollection<TruckCompanies_Shipments> TruckCompanies_Shipments { get; set; }
    }
}