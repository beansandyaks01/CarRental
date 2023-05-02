using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;

namespace HajurKoCarRental.Data
{
    public class EmailHelper
    {
        private readonly string _fromMail;
        private readonly string _fromPassword;
        private readonly MailMessage _mailMessage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public EmailHelper(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _fromMail = configuration["EmailSettings:FromEmail"];
            _fromPassword = configuration["EmailSettings:FromPassword"];
            _mailMessage = new MailMessage();
            _mailMessage.From = new MailAddress(_fromMail);
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task SendEmailAsync(string userId, string subject, string message)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                _mailMessage.Subject = subject;
                _mailMessage.To.Add(new MailAddress(user.Email));
                _mailMessage.Body = $"{message}";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromMail, _fromPassword),
                    EnableSsl = true,
                    Timeout = 10000
                };

                smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }

        public async Task SendRegistrationSuccessEmailAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                _mailMessage.Subject = "Registration Successful";
                _mailMessage.To.Add(new MailAddress(user.Email));
                _mailMessage.Body = "Dear " + user.FirstName + ",\n\nYour registration has been successful.";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromMail, _fromPassword),
                    EnableSsl = true,
                    Timeout = 10000
                };

                smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }
        
        public async Task SendChangePasswordEmailAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                _mailMessage.Subject = "Password Reset";
                _mailMessage.To.Add(new MailAddress(user.Email));
                _mailMessage.Body = "Dear " + user.FirstName + ",\n\nYour password has been changed successfully!";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromMail, _fromPassword),
                    EnableSsl = true,
                    Timeout = 10000
                };

                smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }
        
        public async Task SendRentApproveEmailAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                _mailMessage.Subject = "Request Approved";
                _mailMessage.To.Add(new MailAddress(user.Email));
                _mailMessage.Body = "Dear " + user.FirstName + ",\n\nYour request for vehicle rental has been approved!";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromMail, _fromPassword),
                    EnableSsl = true,
                    Timeout = 10000
                };

                smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }


        public async Task SendOfferMail(Guid id, string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var offer = await _dbContext.SpecialOffers.FindAsync(id);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }
                
                _mailMessage.Subject = offer.OfferTitle;
                _mailMessage.To.Add(new MailAddress(user.Email));
                _mailMessage.Body = "Dear " + user.FirstName + ",\n\n" +offer.Description;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromMail, _fromPassword),
                    EnableSsl = true,
                    Timeout = 10000
                };

                smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }



    }
}