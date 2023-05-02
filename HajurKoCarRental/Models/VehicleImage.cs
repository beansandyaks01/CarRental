using HajurKoCarRental.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace HajurKoCarRental.Models
{
    public class VehicleImage
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Guid VehicleIMG { get; set; }

        [ForeignKey("VehicleId")]
        public Guid VehicleId { get; set; }

        public virtual Vehicle? Vehicle { get; set; }

    }
}
