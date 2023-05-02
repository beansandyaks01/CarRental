using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HajurKoCarRental.Models
{
    public class RentalPayment
    {
        public Guid Id { get; set; }

        [Required]
        public int PaymentType { get; set; }


        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        public decimal TotalAmount   { get; set; }

        [Required]
        public Guid RentId { get; set; }

        [Required]
        public bool IsPaid { get; set; } = false;

        [ForeignKey("RentId")]
        public virtual Rent Rent { get; set; }
    }
}
