using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.DTOs.UserDTOs
{
    public class AddUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
