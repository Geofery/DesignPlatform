using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Shared.Contracts;
using UserManagementService.Application.DTOs;

namespace UserManagementService.Web.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMessageSession _messageSession;

    public UserController(IMessageSession messageSession)
    {
        _messageSession = messageSession;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequest request)
    {
        var userId = Guid.NewGuid();
        var userCreatedEvent = new UserCreatedEvent
        {
            UserId = userId,
            Password = request.Password,
            Email = request.Email,
            Role = request.Role
        };

        await _messageSession.Publish(userCreatedEvent);
        return Ok(new { Message = "User created and event published", UserId = userId });
    }
}
