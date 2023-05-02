using HajurKoCarRental.Data;
using HajurKoCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace HajurKoCarRental.Services
{
    public class VehicleService
    {
        private readonly ApplicationDbContext _dbContext;
        public VehicleService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Vehicle>> GetAllVehicle()
        {
            var vehicles = await _dbContext.Vehicles.ToListAsync();
            return vehicles;
        }
    }
}
