using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.Models
{
    public class CompanyStock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public ICollection<CompanyStocks_Shipments> CompanyStocks_Shipments { get; set; }
    }
}