using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HajurKoCarRental.DTOs.SpecialOfferDTOs
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateSpecialOfferDTO : ControllerBase
    {
        public bool IsValid { get; set; }
    }
}
