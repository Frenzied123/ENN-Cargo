using System.ComponentModel.DataAnnotations;

namespace ENN_Cargo.Models
{
    public class UserSettingsViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only numbers.")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "The password must contain at least one uppercase letter and one number.")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "The password must contain at least one uppercase letter and one number.")]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Experience { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string Role { get; set; }
    }
}
