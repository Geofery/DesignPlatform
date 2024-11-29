using UserManagementService.Application.DTOs;

namespace UserManagementService.Application.Interfaces;

public interface IUserService
{
    Task<LoginRequest?> ValidateUserAsync(string email, string password);
    Task<UserDTO?> GetUserByIdAsync(Guid userId);
    Task<bool> CreateUserAsync(UserDTO user);
}

