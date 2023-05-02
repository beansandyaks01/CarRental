namespace HajurKoCarRental.DTOs.RentDTOs
{
    public class UpdateRentDTO
    {
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public int RentStatus { get; set; }
    }
}
