using Microsoft.EntityFrameworkCore;
using UserManagementService.Application.DTOs;
using UserManagementService.Application.Interfaces;
using UserManagementService.Infrastructure.Repositories;
using UserManagementService.Domain.Models;

namespace UserManagementService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManagementDbContext _dbContext;

    public UserService(UserManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LoginRequest?> ValidateUserAsync(string email, string password)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return new LoginRequest
            {
                Email = user.Email,
                Password = password
            };
        }
        return null;
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null) return null;

        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<bool> CreateUserAsync(UserDTO user)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
        {
            return false;
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash),
            Role = user.Role
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
