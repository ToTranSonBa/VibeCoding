using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyApp.Api.Dtos.Auth;
using StudyApp.Api.Models;
using StudyApp.Api.Services;

namespace StudyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Invalid registration request.");

            var existing = await _userManager.FindByNameAsync(req.UserName!);
            if (existing != null)
                return Conflict("Username already taken.");

            var user = new ApplicationUser
            {
                UserName = req.UserName,
                Email = req.Email,
            };

            var result = await _userManager.CreateAsync(user, req.Password!);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            var dto = new UserDto { Id = user.Id.ToString(), UserName = user.UserName, Email = user.Email };
            return CreatedAtAction(nameof(Register), dto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.UserNameOrEmail) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Invalid login request.");

            ApplicationUser? user = null;
            if (req.UserNameOrEmail!.Contains("@"))
                user = await _userManager.FindByEmailAsync(req.UserNameOrEmail);
            else
                user = await _userManager.FindByNameAsync(req.UserNameOrEmail);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            var check = await _signInManager.CheckPasswordSignInAsync(user, req.Password!, lockoutOnFailure: false);
            if (!check.Succeeded)
                return Unauthorized("Invalid username or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            var response = new AuthResponse
            {
                AccessToken = token,
                ExpiresIn = _tokenService.GetExpiryMinutes() * 60,
                User = new UserDto { Id = user.Id.ToString(), UserName = user.UserName, Email = user.Email }
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.CurrentPassword) || string.IsNullOrWhiteSpace(req.NewPassword))
                return BadRequest("Invalid request.");

            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, req.CurrentPassword!, req.NewPassword!);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return NoContent();
        }
    }
}
