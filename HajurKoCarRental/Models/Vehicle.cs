using System.ComponentModel.DataAnnotations;

namespace HajurKoCarRental.Models
{
    public class Vehicle
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? VehicleNo { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }
        [Required]

        public string? Brand { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public int NoOfSeat { get; set; }

        [Required]
        public int RentPerDay { get; set; }

        [Required]
        public int FuelType { get; set; }


        public bool isAvailable { get; set; } = true;
        
    }
}
