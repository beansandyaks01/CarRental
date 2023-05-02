using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        
        public string Address { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The phone number must be 10 digits long.")]
        public string? PhoneNumber { get; set; }
    }
}
