using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HajurKoCarRental.Controllers;
using HajurKoCarRental.Data;

namespace HajurKoCarRental.Models
{
    public class DamageRequest
    {
        public Guid Id { get; set; }

        public DateTime DamageRequestDate { get; set; } = DateTime.Now;

        [Required]
        public string? Description { get; set; }

        [Required]
        public int Amount { get; set; }


        [Required]
        public string VerifiedBy { get; set; }

        [Required]
        public Guid RentId { get; set; }
        
        [ForeignKey("RentId")]
        public virtual Rent Rent { get; set; }

        [ForeignKey("Verify_by")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}