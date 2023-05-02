using HajurKoCarRental.Data;
using HajurKoCarRental.DTOs.SpecialOfferDTOs;
using HajurKoCarRental.DTOs.VehicleDTOs;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HajurKoCarRental.Controllers
{
    [Route("offers/")]
    [ApiController]

    public class SpecialOfferController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailHelper _emailHelper;

        public SpecialOfferController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, EmailHelper emailHelper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _emailHelper = emailHelper;
        }

        [HttpGet("view-all")]
        public async Task<IActionResult> GetAllSpecialOffers()
        {

            var response = await _dbContext.SpecialOffers.ToListAsync();
            return Ok(response);
        }

        [HttpGet("view-offer-{id}")]
        public async Task<IActionResult> GetSpecialOfferById(Guid id)
        {
            var response = await _dbContext.SpecialOffers.FindAsync(id);
            return Ok(response);
        }


        [HttpPost("publish-offer")]
        public async Task<IActionResult> InsertOffer(InsertSpecialOfferDTO data)
        {
            var newOffer = new SpecialOffer
            {
                Id = Guid.NewGuid(),
                OfferTitle = data.OfferTitle,
                Description = data.Description,
                StartDate = data.StartDate,
                OfferDuration = data.OfferDuration,
                DiscountPercent = data.DiscountPercent,

            };

            _dbContext.SpecialOffers.Add(newOffer);
            _dbContext.SaveChanges();

            // To get users
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                List<string> userRole = (List<string>)await _userManager.GetRolesAsync(user);
                var customer = await _userManager.IsInRoleAsync(user, "ApplicationUser");

                List<string> userIds = new List<string>();
                if (customer)
                {

                    userIds.Add(user.Id);
                    await _emailHelper.SendOfferMail(newOffer.Id, user.Id);

                }
            }
            
            //var userRole = _dbContext.UserRoles.ToList();

            return Ok(newOffer);
        }

        

        [HttpPost("expire-offer-{id}")]
        public async Task<IActionResult> Expire(Guid id)
        {
            var offer = await _dbContext.SpecialOffers.FindAsync(id);
            var validity = (int)(offer.StartDate - DateTime.Now).TotalDays;
            if (offer == null)
            {
                throw new Exception("Offer not found");
            }
            if (validity <= 0)
            {
                offer.IsValid = false;
                throw new Exception("Offer has expired");
            }
            
            _dbContext.SpecialOffers.Update(offer);
            _dbContext.SaveChanges();
            return Ok(offer);
        }

    }
}
