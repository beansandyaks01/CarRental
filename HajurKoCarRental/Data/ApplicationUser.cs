using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address   { get; set; }
    }
    
    public class ApplicationRole:IdentityRole
    {

    }
}
