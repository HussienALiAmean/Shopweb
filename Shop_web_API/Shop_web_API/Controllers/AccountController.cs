using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop_web_API.Models;
using Shop_web_API.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop_web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Assign role based on input
                    var role = model.Role ?? "User"; // Default to "User" if no role is provided
                    try
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        return Ok(new { message = "User registered successfully!" });

                    }
                    catch (Exception)
                    {
                        return BadRequest("Invalid role specified.");
                    }
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var tokenString = GenerateJWT(user);
                    return Ok(new { token = tokenString });
                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }

            private string GenerateJWT(ApplicationUser user)
            {
                // Retrieve user roles
                var roles = _userManager.GetRolesAsync(user).Result;

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("UserId", user.Id),
                }
                .Union(roles.Select(role => new Claim("Roles", role)));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtIssuer"],
                    claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            // Send email with the reset link
            // _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            return Ok(new { message = "Password reset link has been sent to your email." });
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "Password has been reset." });
            }

            return BadRequest(result.Errors);
        }
    }
}
