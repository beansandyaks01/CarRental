using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.Models
{
    public class SpecialOffer
    {
        [Key]
        public Guid Id { get; set; }


        [Required]
        public string? OfferTitle { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        public int DiscountPercent { get; set; }

        [Required]
        public int OfferDuration { get; set; }

        public bool IsValid { get; set; } = true;
    }
}
