using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.DTOs.RentPaymentDTOs
{
    public class InsertRentalPaymentDTO
    {
        public int PaymentType { get; set; }

        public int TotalAmount { get; set; }
        public int Discount { get; set; }

        public Guid RentId { get; set; }


    }
}
