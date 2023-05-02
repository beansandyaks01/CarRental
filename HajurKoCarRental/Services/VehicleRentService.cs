using HajurKoCarRental.Data;
using HajurKoCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace HajurKoCarRental.Services
{
    public class VehicleRentService
    {
        private readonly ApplicationDbContext _dbContext;
        public VehicleRentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Rent>> GetAllVehicle()
        {
            var rents = await _dbContext.Rents.ToListAsync();
            return rents;
        }
    }



}
