using HajurKoCarRental.Controllers;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HajurKoCarRental.Data
{
    public class ApplicationDbContext :IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ):base(options)
        {

        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<DamageRequest> DamageRequests { get; set; }
        public DbSet<DamagePayment> DamagePayments { get; set; }
        public DbSet<RentalPayment> RentalPayments { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<VehicleImage> VehicleImages { get; set; }
    }
}
