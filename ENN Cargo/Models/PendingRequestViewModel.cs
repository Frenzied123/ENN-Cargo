namespace ENN_Cargo.Models
{
    public class PendingRequestViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Request { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
    }
}