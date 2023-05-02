using HajurKoCarRental.Data;
using HajurKoCarRental.DTOs.DamagePaymentDTOs;
using HajurKoCarRental.DTOs.RentPaymentDTOs;
using HajurKoCarRental.DTOs.VehicleDTOs;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HajurKoCarRental.Controllers
{
    [Route("damagepayment/")]
    [ApiController]
    public class DamagePaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public DamagePaymentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("view-all")]
        public async Task<IActionResult> GetDamagePayment()
        {

            var response = await _dbContext.DamagePayments.ToListAsync();
            return Ok(response);
        }

        [HttpGet("view-{id}")]
        public async Task<ActionResult<DamagePayment>> GetDamagePaymentID(Guid id)
        {
            var response = await _dbContext.DamagePayments.FindAsync(id);
            return Ok(response);
        }

        [HttpPost("payment")]
        public async Task<ActionResult<DamagePayment>> InsertDamagePayment(InsertDamagePaymentDTO data)
        {
            var newPayment = new DamagePayment
            {
                Id = Guid.NewGuid(),
                PaymentType = data.PaymentType,
                PaidAmount = data.PaidAmount,
                DamageRequestId = data.DamageRequestId,
                IsPaid = true

            };
            _dbContext.DamagePayments.Add(newPayment);
            _dbContext.SaveChanges();
            return Ok(newPayment);
        }


        [HttpGet("damage-history")]
        public IActionResult GetDamageHistory()
        {
            var damageRequests = _dbContext.DamageRequests.Include(dr => dr.Rent.VechileId).Include(dr => dr.ApplicationUser).ToList();

            var damageHistory = damageRequests.Select(dr =>
            {
                var customerName = dr.ApplicationUser != null ? $"{dr.ApplicationUser.FirstName} {dr.ApplicationUser.LastName}" : "Unknown";
                var paidAmount = _dbContext.DamagePayments.Where(dp => dp.DamageRequestId == dr.Id && dp.IsPaid).Sum(dp => dp.PaidAmount);
                var unpaidAmount = dr.Amount - paidAmount;
                return new
                {
                    VehicleName = dr.Rent.Vehicle.Name,
                    VehicleBrand = dr.Rent.Vehicle.Brand,
                    CustomerName = customerName,
                    EstimatedCost = dr.Amount,
                    PaidAmount = paidAmount,
                    UnpaidAmount = unpaidAmount
                };
            }).ToList();

            return Ok(damageHistory);
        }
    }
}

