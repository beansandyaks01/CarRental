using HajurKoCarRental.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HajurKoCarRental.Models
{
    public class Rent
    {
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime RentalDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime RentRequestDate { get; set; }
        
        [Required]
        public int RentDuration { get; set; }

        [Required]
        public int RentStatus { get; set; }

        [Required]
        public int Discount { get; set; }

        [Required]
        public bool IsApproved { get; set; } = false;

        [Required]
        public bool IsAvailable { get; set; } = true;
        
        public string? ApprovedBy { get; set; }

        public Guid VechileId { get; set; }
        public string? CustomerId { get; set; }
      
        [ForeignKey("VechileId")]
        public virtual Vehicle? Vehicle { get; set; }
        
        [ForeignKey("CustomerId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        

        [ForeignKey("ApprovedBy")]
        public virtual ApplicationUser? ApplicationStaff { get; set; }
    }
}
