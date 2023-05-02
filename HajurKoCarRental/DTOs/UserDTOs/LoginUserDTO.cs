using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.DTOs.UserDTOs
{
    public class LoginUserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
