using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Experience { get; set; }
        public string PhoneNumber { get; set; }
        public int TruckCompany_Id { get; set; }
        public TruckCompany TruckCompany { get; set; }
    }
}
