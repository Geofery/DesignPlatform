using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PaymentService.Web
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        [Authorize] // Requires a valid JWT token
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "This is a secure endpoint. You are authorized!",
                UserId = userId,
                Role = role
            });
        }

        [AllowAnonymous] // No token required
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("This is a public endpoint. Anyone can access it.");
        }
    }

}

