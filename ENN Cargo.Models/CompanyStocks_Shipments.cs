using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.Models
{
    public class CompanyStocks_Shipments
    {
        public int CompanyStock_Id { get; set; }
        public CompanyStock CompanyStock { get; set; }
        public int Shipment_Id { get; set; }
        public Shipment Shipment { get; set; }
    }
}