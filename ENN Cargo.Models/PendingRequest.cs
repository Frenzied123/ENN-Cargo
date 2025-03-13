using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENN_Cargo.Models
{
    public class PendingRequest
    {
        public int Id { get; set; }
        public string Type { get; set; } 
        public string Status { get; set; } = "Pending";
        public string UserId { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
