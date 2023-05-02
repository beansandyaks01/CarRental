using HajurKoCarRental.Data;
using HajurKoCarRental.DTOs.RentPaymentDTOs;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HajurKoCarRental.Models.RentalPayment;

namespace HajurKoCarRental.Controllers
{
    [Route("rentalpayments/")]
    [ApiController]
    public class RentalPaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentalPaymentController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        [HttpGet("view-all")]
        public async Task<IActionResult> GetRentalPayment()
        {

            var response = await _dbContext.RentalPayments.ToListAsync();
            return Ok(response);
        }

       

        /*[HttpGet("view-{id}")]
        public async Task<ActionResult<RentalPayment>> GetRentalPaymentID(Guid id)
        {
            var response = await _dbContext.RentalPayments.FindAsync(id);
            return Ok(response);
        }*/


        [HttpPost("pay")]
        public async Task<ActionResult<RentalPayment>> InsertRentalPayment(Guid requestId, InsertRentalPaymentDTO data)
        {
            var rentPayment = _dbContext.RentalPayments.FirstOrDefault(x => x.Id == requestId);
            var rentRequest = _dbContext.Rents.FirstOrDefault(x => x.Id == requestId);
            var vehicle = _dbContext.Vehicles.FirstOrDefault(x => x.Id == rentRequest.VechileId);
            var specialOffer = _dbContext.SpecialOffers.FirstOrDefault(x => x.Id == x.Id);
            var paidAmount = _dbContext.RentalPayments.FirstOrDefault(x => x.RentId == requestId);
            var rents = _dbContext.Rents.Where(x => x.RentalDate >= DateTime.UtcNow.AddMonths(-1)).ToList();




            var userId = rentRequest.CustomerId;


            var user = await _userManager.FindByIdAsync(userId);
            var userRole = await _userManager.GetRolesAsync(user);


            var customer = await _userManager.IsInRoleAsync(user, "ApplicationUser");


            //Add logic to check for regular customer

            var regularCustomer = rents.Count(x => x.CustomerId == userId);


            if (paidAmount == null)
            {
                int days = (int)(rentRequest.RentalDate - DateTime.Now).TotalDays;
                int lateDays = days - rentRequest.RentDuration;
                decimal rentAmount = rentRequest.RentDuration * vehicle.RentPerDay;
                decimal fineAmount = lateDays * (vehicle.RentPerDay * (decimal)2.0);
                decimal totalAmount = rentAmount + fineAmount;


                if (userRole[0] == "StaffUser")
                {
                    rentRequest.Discount = 25;
                    decimal discountAmount = totalAmount * 25 / 100;
                    totalAmount -= discountAmount;
                    rentPayment = new RentalPayment
                    {
                        Id = Guid.NewGuid(),
                        PaymentType = 1,
                        TotalAmount = totalAmount,
                        RentId = data.RentId,
                    };

                }
                else if (regularCustomer >= 1)
                {
                    rentRequest.Discount = 10;
                    decimal discountAmount = totalAmount * 10 / 100;
                    totalAmount -= discountAmount;
                    rentPayment = new RentalPayment
                    {
                        Id = Guid.NewGuid(),
                        PaymentType = 1,
                        TotalAmount = totalAmount,
                    };
                }


                if (specialOffer != null && specialOffer.IsValid && specialOffer.StartDate <= DateTime.Now && 
                    specialOffer.StartDate.AddDays(specialOffer.OfferDuration) >= DateTime.Now)
                {
                    decimal discountAmount = totalAmount * specialOffer.DiscountPercent / 100;
                    totalAmount -= discountAmount;
                }
                rentPayment = new RentalPayment
                {
                    Id = Guid.NewGuid(),
                    PaymentType = 1,
                    TotalAmount = totalAmount,
                    RentId = data.RentId,
                };

                await _dbContext.RentalPayments.AddAsync(rentPayment);
                _dbContext.SaveChanges();
            }

            return Ok(rentPayment);

        }
    }
}
