using Microsoft.AspNetCore.Mvc;
using UserManagementService.Domain;
using UserManagementService.Infrastructure;


namespace UserManagementService.Web

{ 
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService; 

    public AuthController(ITokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.ValidateUserAsync(request.Email, request.Password);
        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }

        var token = _tokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = request.Password,
            Role = request.Role
        };

        var success = await _userService.CreateUserAsync(user);
        if (!success)
        {
            return BadRequest("User already exists");
        }

        return Ok("User registered successfully");
    }
}
}


