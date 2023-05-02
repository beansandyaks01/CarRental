namespace HajurKoCarRental.DTOs.SpecialOfferDTOs
{
    public class InsertSpecialOfferDTO
    {
        public string OfferTitle { get; set; }

        public string Description { get; set; }


        public DateTime StartDate { get; set; }

        public int DiscountPercent { get; set; }

        public int OfferDuration { get; set; }

    }
}
