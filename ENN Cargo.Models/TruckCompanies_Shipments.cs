using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;namespace ENN_Cargo.Models
{
    public class TruckCompanies_Shipments
    {
        public int TruckCompany_Id { get; set; }
        public TruckCompany TruckCompany { get; set; }
        public int Shipment_Id { get; set; }
        public Shipment Shipment { get; set; }
    }
}