using HajurKoCarRental.Data;
using HajurKoCarRental.DTOs.DamageDTOs;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HajurKoCarRental.Controllers
{
    [Route("damage-request/")]
    [ApiController]
    public class DamageRequestController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public DamageRequestController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("view-all")]
        public async Task<IActionResult> GetAllDamageRequests()
        {

            var response = await _dbContext.DamageRequests.ToListAsync();
            return Ok(response);
        }

        [HttpGet("view-{damageId}")]
        public async Task<IActionResult> GetDamageRequestById(Guid damageId)
        {
            var response = await _dbContext.DamageRequests.FindAsync(damageId);
            return Ok(response);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddDamageRequest(InsertDamageDTO data)
        {
            var newDamageRequest = new DamageRequest
            {
                Id = Guid.NewGuid(),
                Description = data.Description,
                RentId = data.RentId,
            };

            _dbContext.DamageRequests.Add(newDamageRequest);
            _dbContext.SaveChanges();
            return Ok(newDamageRequest);
        }


        [HttpPost("verify")]
        public async Task<IActionResult> VerifyDamageRequest( Guid id, string userId, UpdateDamageDTO data)
        {
            var damage = await _dbContext.DamageRequests.FindAsync(id);

            var currentUser = await _userManager.FindByIdAsync(userId);
            var userRole = await _userManager.GetRolesAsync(currentUser);
            if (userRole[0] == "StaffUser" || userRole[0] == "AdminUser")
            {
                if (damage == null)
                {
                    throw new Exception("Invalid vehicle id");
                }
                damage.Amount = data.Amount;
                damage.VerifiedBy = currentUser.FirstName + " " + currentUser.LastName;
                return Ok(damage);
            }
            else
            {
                return BadRequest("UnAuthorized to verify damage");
            }
                
        }


        

        
    }
}
