using HajurKoCarRental.Data;
using HajurKoCarRental.DTOs.RentDTOs;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HajurKoCarRental.Controllers;

[Route("rent/")]
[ApiController]
public class VehicleRentController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly EmailHelper _emailHelper;
    public VehicleRentController(ApplicationDbContext dbContext,  EmailHelper emailHelper)
    {
        _dbContext = dbContext;

        _emailHelper = emailHelper;
    }

    [HttpGet("view-all")]
    public async Task<IActionResult> GetAllRents()
    {
        var response = await _dbContext.Rents.ToListAsync();
        return Ok(response);
    }


    [HttpGet("view-{Rent_id}")]
    public async Task<IActionResult> GetRentById(Guid Rent_id)
    {
        var response = await _dbContext.Rents.FindAsync(Rent_id);
        return Ok(response);
    }

    [HttpPost("request")]
    public async Task<IActionResult> RentVehicle( InsertRentDTO data)
    {
        // Check if the customer has any outstanding rental bills or damage fees
        
        var customer = _dbContext.Users
         .SingleOrDefault(c => c.Id == data.CustomerId);

        if (customer != null)
        {
            // Check if the customer has any unpaid rental bills
            var unpaidBills = _dbContext.RentalPayments
                .Where(rp => rp.Rent.CustomerId == customer.Id && rp.IsPaid == false)
                .ToList();
            if (unpaidBills.Count > 0)
            {
                return BadRequest("Customer has unpaid rental bills and is not eligible for a new rental.");
            }

            // Check if the customer has any unpaid damage fees
            var unpaidFees = _dbContext.DamagePayments
                .Where(dp => dp.DamageRequest.Rent.CustomerId == customer.Id && dp.IsPaid == false)
                .ToList();
            if (unpaidFees.Count > 0)
            {
                return BadRequest("Customer has unpaid damage fees and is not eligible for a new rental.");
            }
        }
        else
        {
            return BadRequest("Customer Not Found");
        }

        /*var vehicle = _dbContext.Vehicles
            .SingleOrDefault(v => v.Id == data.VechileId);

        if (vehicle != null)
        {
            // Check if the vehicle has any unpaid rental bills or damage fees
            var unpaidBills = _dbContext.RentalPayments
                .Where(rentPayment => rentPayment.Rent.VechileId == vehicle.Id && rentPayment.IsPaid == false)
                .ToList();
            var unpaidFees = _dbContext.DamagePayments
                .Where(damagePayment => damagePayment.DamageRequest.RentId == vehicle.Id && damagePayment.IsPaid == false)
                .ToList();
            if (unpaidBills.Count > 0 || unpaidFees.Count > 0)
            {
                vehicle.isAvailable = true;
                _dbContext.SaveChanges();
                return BadRequest("You cannot rent further. Please clear your dues to rent!");
            }           
        }else
        {
            return BadRequest("Vehicle Not Found");
        }*/
        var newRent = new Rent
        {
            Id = Guid.NewGuid(),
            RentalDate = data.RentalDate,
            RentRequestDate = data.RentRequestDate,
            RentDuration = data.RentDuration,
            RentStatus = 1,
            VechileId = data.VechileId,
            CustomerId = data.CustomerId,
            IsAvailable = false,
            ApprovedBy = string.IsNullOrEmpty(data.ApprovedBy) ? null : data.ApprovedBy

        };

        _dbContext.Rents.Add(newRent);
        _dbContext.SaveChanges();

        return Ok(newRent);
    }

    [HttpDelete("cancel-request")]
    public async Task<IActionResult> CancelRentRequest(Guid Rent_id)
    {
        try
        {
            var rent = await _dbContext.Rents.FindAsync(Rent_id);
            if (rent == null)
            {
                throw new Exception("Invalid vehicle id");
            }

            // Cancel the rent and update the database
            rent.RentStatus = 0;
            rent.IsAvailable = true;
            _dbContext.Rents.Update(rent);
            _dbContext.SaveChanges();

            return Ok("Rent cancelled successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("approve-request/{id}")]
    public async Task<IActionResult> ApproveRent(Guid id, string userId)
    {
        var rent = await _dbContext.Rents.FindAsync(id);

        var currentUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

        if (rent == null)
        {
            return NotFound();
        }
        

        if (currentUser == null)
        {
            return Unauthorized();
        }

        rent.IsAvailable = false;
        rent.RentStatus = 2;
        rent.IsApproved= true;
        rent.ApprovedBy = currentUser.Id;

        await _emailHelper.SendRentApproveEmailAsync(rent.CustomerId);
        _dbContext.SaveChanges();

        return Ok(rent);
    }

    [HttpDelete("delete-request")]
    public async Task<IActionResult> RemoveRent(Guid Rent_id)
    {
        try
        {
            var rent = await _dbContext.Rents.FindAsync(Rent_id);
            if (rent == null)
            {
                throw new Exception("Invalid vehicle id");
            }
            _dbContext.Rents.Remove(rent);
            _dbContext.SaveChanges();
            return Ok("Suucessfully deleted");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    //Done by staff or admin
    [HttpPatch("return")]
    public async Task<IActionResult> ReturnVehicle(InsertRentDTO data, Guid Rent_id)
    {
        var rent = await _dbContext.Rents.FindAsync(Rent_id);
        try
        {
            if (rent == null)
            {
                throw new Exception("Invalid vehicle id");
            }
            rent.IsAvailable = true;
            _dbContext.Rents.Update(rent);
            _dbContext.SaveChanges();
            return Ok(rent);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }




    [HttpGet("rent-details")]
    public IActionResult GetRentDetails()
    {
        var rents = _dbContext.Rents.ToList();
        var users = _dbContext.Users.ToList();
        var rentPayments = _dbContext.RentalPayments.ToList();

        var history = (from rent in rents
                       join customer in users
                      on rent.CustomerId equals customer.Id
                       join staff in users
                     on rent.ApprovedBy equals staff.Id
                       join rentPayment in rentPayments
                       on rent.Id equals rentPayment.RentId
                       orderby rent.RentalDate descending
                       select new
                       {
                           CustomerId = customer.Id,
                           CustomerName = customer.FirstName + " " + customer.LastName,
                           RentId = rent.Id,
                           RentDate = rent.RentalDate,
                           RentPaymentId = rentPayment.Id,
                           RentCharge = rentPayment.TotalAmount,
                           VehicleId = rent.VechileId,
                           ApprovedBy = staff.Id,
                       }).ToList();
        return Ok(history);
    }

    [HttpGet("currently-rented")]
    public IActionResult GetAllCurrentlyRented()
    {
        var rents = _dbContext.Rents.ToList();
        var vehicles = _dbContext.Vehicles.ToList();

        // Retrieve all cars currently on rent
        var carsOnRent = from rent in rents
                         join vehicle in vehicles on rent.VechileId equals vehicle.Id
                         where rent.RentStatus == 2
                         select vehicle;
        return Ok(carsOnRent);
    }
    
    [HttpGet("available-cars")]
    public IActionResult GetAvailableCars()
    {
        var rents = _dbContext.Rents.ToList();
        var vehicles = _dbContext.Vehicles.ToList();

        // Retrieve all available cars
        var availableCars = from vehicle in vehicles
                            where !rents.Any(r => r.VechileId == vehicle.Id && r.RentStatus == 2)
                            select vehicle;

        return Ok(availableCars);
    } 
    
    
    [HttpGet("frequently-rented-cars")]
    public IActionResult GetFrequentlyRentedCars()
    {
        var rents = _dbContext.Rents.ToList();
        var vehicles = _dbContext.Vehicles.ToList();

        // Retrieve frequently rented cars
        var frequentlyRentedCars = rents
                           .GroupBy(r => r.VechileId)
                           .Where(g => g.Count() >= 3)
                           .Select(g => g.Key)
                           .Join(vehicles, r => r, v => v.Id, (r, v) => v);


        return Ok(frequentlyRentedCars);
    }
    
    [HttpGet("not-rented-cars")]
    public IActionResult GetNotRentedCars()
    {
        var rents = _dbContext.Rents.ToList();
        var vehicles = _dbContext.Vehicles.ToList();

        // Retrieve cars that have not been rented
        var notRentedCars = from vehicle in vehicles
                            where !rents.Any(rent => rent.VechileId == vehicle.Id)
                            select vehicle;


        return Ok(notRentedCars);
    }

}