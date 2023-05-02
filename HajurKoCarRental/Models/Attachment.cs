using HajurKoCarRental.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HajurKoCarRental.Models
{
    public class Attachment
    {
        public Guid Id { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public Guid LicenseIMG { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }

    }
}
