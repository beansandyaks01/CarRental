using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.DTOs.DamageDTOs
{
    public class InsertDamageDTO
    {
        [DataType(DataType.Date)]
        public DateTime DamageRequestDate { get; set; } = DateTime.Now;

        public string Description { get; set; }

        public string VerifyBy { get; set; }

        public Guid RentId { get; set; }
    }
}
