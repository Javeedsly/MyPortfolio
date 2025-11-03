using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Core.DTOs;
using MyPortfolio.Core.Entities;
using MyPortfolio.Core.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities; 
using System.Text; 

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService; 

        public AuthController(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IEmailService emailService) 
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService; 
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user.Id, token = encodedToken }, Request.Scheme);

            if (confirmationLink == null)
            {
                return StatusCode(500, "Could not generate confirmation URL.");
            }

            var message = $"<h1>Email Confirmation</h1><p>Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.</p>";
            await _emailService.SendEmailAsync(user.Email, "Confirm your email", message);


            return Ok(new { Message = "Registration successful. Please check your email to confirm your account." });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("User ID and token are required.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Email confirmed successfully. You can now log in." });
            }

            return BadRequest("Email could not be confirmed.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized(new { Message = "Email not confirmed. Please check your inbox." });
            }

            var token = await _tokenService.CreateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.Now.AddDays(7),
                Username = user.UserName
            });
        }
    }
}