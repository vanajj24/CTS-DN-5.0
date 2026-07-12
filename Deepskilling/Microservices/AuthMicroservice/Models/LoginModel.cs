using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        // Added to allow selecting user vs admin role during test requests
        public string Role { get; set; } = "User"; 
    }
}
