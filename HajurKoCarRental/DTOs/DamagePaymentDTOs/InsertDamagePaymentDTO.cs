namespace HajurKoCarRental.DTOs.RentPaymentDTOs
{
    public class InsertDamagePaymentDTO
    {
        public int PaidAmount { get; set; }
        public int PaymentType { get; set; }

        public Guid DamageRequestId { get; set; }
    }
}
