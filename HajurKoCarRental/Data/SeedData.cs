using System;
using System.Collections.Generic;
using System.Linq;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HajurKoCarRental.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
                        RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            if (!_context.Roles.Any())
            {
                // Seed roles
                var roles = new List<IdentityRole>
        {
            new IdentityRole("AdminUser"),
            new IdentityRole("StaffUser"),
            new IdentityRole("ApplicationUser")
        };

                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            if (!_context.Users.Any())
            {
                // Seeding users details 
                var adminUser = new ApplicationUser
                {
                    UserName = "binayakmaharjan2000@gmail.com",
                    Email = "binayakmaharjan2000@gmail.com",
                    FirstName = "Rex",
                    LastName = "Heart",
                    Address = "Sanepa",
                    PhoneNumber = "9812345670",
                };

                var staffUser = new ApplicationUser
                {
                    UserName = "utsavkhadgi100@gmail.com",
                    Email = "utsavkhadgi100@gmail.com",
                    FirstName = "Utsav",
                    LastName = "Khadgi",
                    Address = "Jhamsikhel",
                    PhoneNumber = "9812345678",

                };

                var applicationUser = new ApplicationUser
                {
                    UserName = "binayakmaharjan01@gmail.com",
                    Email = "binayakmaharjan01@gmail.com",
                    FirstName = "Tyson",
                    LastName = "Jackson",
                    Address = "Kupondole",
                    PhoneNumber = "9812345679"
                };

                await _userManager.CreateAsync(adminUser, "Rex@123");
                await _userManager.CreateAsync(staffUser, "Utsav@123");
                await _userManager.CreateAsync(applicationUser, "Tyson@123");

                // Assign roles to users in AspNetRoles
                await _userManager.AddToRoleAsync(adminUser, "AdminUser");
                await _userManager.AddToRoleAsync(staffUser, "StaffUser");
                await _userManager.AddToRoleAsync(applicationUser, "ApplicationUser");
            }
        }
    }
}