using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HajurKoCarRental.Models
{
    public class DamagePayment
    {
        public Guid Id { get; set; }
        [Required]
        public int PaidAmount { get; set; }
        [Required]
        public int PaymentType { get; set; }

        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        public Guid DamageRequestId { get; set; }

        [Required]
        public bool IsPaid { get; set; } = false;

        [ForeignKey("DamageRequestId")]
        public virtual DamageRequest DamageRequest { get; set; }


    }
}
