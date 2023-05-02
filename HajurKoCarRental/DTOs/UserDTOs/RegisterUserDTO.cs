using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.DTOs.UserDTOs
{
    public class RegisterUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Address { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "The phone number must be 10 digits long.")]
        public string PhoneNumber { get; set; }

    }
}
