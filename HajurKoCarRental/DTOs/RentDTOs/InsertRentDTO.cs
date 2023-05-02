namespace HajurKoCarRental.DTOs.RentDTOs
{
    public class InsertRentDTO
    {
        public DateTime RentalDate { get; set; }
        public DateTime RentRequestDate { get; set; }

        public int RentDuration { get; set; }

        public int RentStatus { get; set; }
        public string? ApprovedStatus { get; set; }

        public Guid VechileId { get; set; }

        public string CustomerId { get; set; }
        public string ApprovedBy { get; set; }


    }
}
