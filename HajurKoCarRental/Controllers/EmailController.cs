using HajurKoCarRental.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("email/")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly EmailHelper _emailHelper;
    private readonly ApplicationDbContext _dbContext;

    public EmailController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext dbContext)
    {
        _emailHelper = new EmailHelper(userManager, configuration, dbContext);
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail(string userId, string subject, string message)
    {
        try
        {
            await _emailHelper.SendEmailAsync(userId, subject, message);
            return Ok("Email sent successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}