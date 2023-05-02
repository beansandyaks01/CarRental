using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HajurKoCarRental.Data;
using HajurKoCarRental.DTOs.UserDTOs;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;

namespace HajurKoCarRental.Controllers
{
    [Route("user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private Account _account;
        private Cloudinary _cloudinary;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailHelper _emailHelper;

        public UserController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, 
            EmailHelper emailHelper)
        {
            _dbContext = dbContext;
            _account = new Account { Cloud = "dnow8alub", ApiKey = "719549538397893", ApiSecret = "9LasXvfPK7TrvufQjkk4Q7ZGfhs" };
            _cloudinary = new Cloudinary(_account);
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailHelper = emailHelper;

            //_applicationManager = applicationManager;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO data)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync("ApplicationUser");

                var iuser = new ApplicationUser
                {
                    UserName = data.Username,
                    Email = data.Username,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Address = data.Address,
                    PhoneNumber = data.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(iuser, data.Password);

                if (result.Succeeded)
                {
                    var assignRoleResult = await _userManager.AddToRoleAsync(iuser, role.Name);

                    if (assignRoleResult.Succeeded)
                    {
                        await _emailHelper.SendRegistrationSuccessEmailAsync(iuser.Id);
                        return Ok("User registered and assigned to role successfully!");
                    }
                    else
                    {
                        return BadRequest("User registered.");
                    }
                }
                else
                {
                    return BadRequest("Registration Failed!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /*[HttpGet("userRole/{username}")]
        public async Task<IActionResult> GetUserRole(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(username);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                if (string.IsNullOrEmpty(role))
                {
                    throw new Exception("Role not found.");
                }

                var roleObject = await _roleManager.FindByNameAsync(role);

                if (roleObject == null)
                {
                    throw new Exception("Role not found.");
                }

                return Ok(roleObject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/


        /*[HttpGet("roles")]
        public IActionResult GetRoles()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
 

        [HttpPost("add-admin")]
        public async Task<IActionResult> CreateAdmin(RegisterUserDTO data)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync("AdminUser");

                var iuser = new ApplicationUser
                {
                    UserName = data.Username,
                    Email = data.Username,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Address = data.Address,
                    PhoneNumber = data.PhoneNumber,


                };

                var result = await _userManager.CreateAsync(iuser, data.Password);
                if (result.Succeeded)
                {
                    var assignRoleResult = await _userManager.AddToRoleAsync(iuser, role.Name);

                    if (assignRoleResult.Succeeded)
                    {
                        await _emailHelper.SendRegistrationSuccessEmailAsync(iuser.Id);
                        return Ok("Admin added successfully");
                    }

                    else
                    {
                        return BadRequest("Admin not registered.");
                    }
                }
                else
                {
                    
                    return Ok("Admin not registered!");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-staff")]
        public async Task<IActionResult> CreateStaff(RegisterUserDTO data)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync("StaffUser");

                var iuser = new ApplicationUser
                {
                    UserName = data.Username,
                    Email = data.Username,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Address = data.Address,
                    PhoneNumber = data.PhoneNumber,


                };

                var result = await _userManager.CreateAsync(iuser, data.Password);
                if (result.Succeeded)
                {
                    var assignRoleResult = await _userManager.AddToRoleAsync(iuser, role.Name);

                    if (assignRoleResult.Succeeded)
                    {
                        await _emailHelper.SendRegistrationSuccessEmailAsync(iuser.Id);
                        return Ok("Staff registered and assigned to staff role.");

                    }

                    else
                    {
                        return BadRequest("Staff not registered.");
                    }
                }
                else
                {
                    return Ok("Staff added successfully!");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-customer")]
        public async Task<IActionResult> CreateCustomer(RegisterUserDTO data)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync("ApplicationUser");

                var iuser = new ApplicationUser
                {
                    UserName = data.Username,
                    Email = data.Username,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Address = data.Address,
                    PhoneNumber = data.PhoneNumber,


                };

                var result = await _userManager.CreateAsync(iuser, data.Password);
                if (result.Succeeded)
                {
                    var assignRoleResult = await _userManager.AddToRoleAsync(iuser, role.Name);

                    if (assignRoleResult.Succeeded)
                    {
                        await _emailHelper.SendRegistrationSuccessEmailAsync(iuser.Id);
                        return Ok("Customer registered successfully");
                    }

                    else
                    {
                        return BadRequest("Customer not registered.");
                    }
                }
                else
                {

                    return Ok("Customer not registered.");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO data)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(data.Username, data.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(data.Username);
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault();

                    return Ok(new { UserId = user.Id, Username = user.UserName, Role = role });
                }
                else
                {
                    throw new Exception("Invalid login attempt.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return Ok("User logged out successfully!");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(string userId, ChangePasswordDTO data)
        {
            try
            {

                // Retrieve the current user using their ID
                var user = await _userManager.FindByIdAsync(userId);

               //Display error message if user does not exist
                if (user == null)
                {
                    throw new Exception("Current user not found.");
                }

                // Validate the current user and their current password
                var result = await _signInManager.CheckPasswordSignInAsync(user, data.CurrentPassword, false);

                if (!result.Succeeded)
                {
                    throw new Exception("Current password is invalid.");
                }

                // Change the user's password
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, data.CurrentPassword, data.NewPassword);

                if (changePasswordResult.Succeeded)
                {
                    await _emailHelper.SendChangePasswordEmailAsync(user.Id);
                    return Ok("Password changed successfully!");
                }
                else
                {
                    throw new Exception("Failed to change password.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpGet("view-profile")]
        public async Task<IActionResult> ViewProfile(string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
            {
                return NotFound();
            }
            var user = await _dbContext.Users.FindAsync(currentUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("view-all-users")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var response = await _dbContext.Users.ToListAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


      
        [HttpPut("update-user-{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDTO data)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Address = data.Address;
            user.PhoneNumber = data.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);

            return Ok(result);

        }


        [HttpPost("upload-license")]
        public async Task<IActionResult> UploadLicenseImage(string userId, IFormFile license)
        {
            //Check if the file has been uploaded
            if (license == null)
            {
                throw new Exception("Attachment not found");

            }
            // Check if the file is in the allowed format
            if (license.ContentType != "image/png" && license.ContentType != "application/pdf")
            {
                return BadRequest("Invalid file format. Only PNG and PDF are allowed.");
            }

            // Check if the file size is within the limit
            if (license.Length > 1.5 * 1024 * 1024)
            {
                return BadRequest("File size exceeds the limit of 1.5 MB.");
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Invalid User");
            }


            var stream = new MemoryStream();
            await license.CopyToAsync(stream);
            stream.Position = 0;
            var imageId = Guid.NewGuid();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(license.FileName, stream),
                PublicId = $"Attachments/{imageId}",
                Transformation = new Transformation().FetchFormat("auto")
            };

            _cloudinary.Upload(uploadParams);

            var newAttachments = new Attachment
            {
                Id = Guid.NewGuid(),
                LicenseIMG = imageId,
                UserId = user.Id
            };
            _dbContext.Attachments.Add(newAttachments);
            _dbContext.SaveChanges();
            return Ok("Attachment added!");
        }



        [HttpGet("view-attachment/{userId}")]
        public IActionResult ViewAttachment(string userId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var attachment = _dbContext.Attachments.FirstOrDefault(a => a.UserId == userId);
            if (attachment == null)
            {
                return NotFound("No attachment found for this user.");
            }

            var UserImageUrl = _cloudinary.Api.UrlImgUp.Secure().BuildUrl($"Attachments/{attachment.LicenseIMG}.png");


            return Redirect(UserImageUrl);
        }


        [HttpGet("frequent-users")]
        public IActionResult GetFrequentUsers()
        {

            var rents = _dbContext.Rents.ToList();
            var users = _dbContext.Users.ToList();

            var frequentCustomers = (from rent in rents
                                     group rent by rent.CustomerId into rentGroup
                                     where rentGroup.Count() >= 3
                                     select rentGroup.Key).ToList();


            return Ok(frequentCustomers);
        }



        [HttpGet("inactive-users")]
        public IActionResult GetInActiveUsers()
        {
            DateTime threeMonthsAgo = DateTime.Today.AddMonths(-3);

            var rents = _dbContext.Rents.ToList();
            var users = _dbContext.Users.ToList();

            var inactiveCustomers = rents
                .Where(rent => rent.RentalDate < threeMonthsAgo)
                .Select(renter => renter.CustomerId)
                .Distinct();


            return Ok(inactiveCustomers);
        }

    }
}
