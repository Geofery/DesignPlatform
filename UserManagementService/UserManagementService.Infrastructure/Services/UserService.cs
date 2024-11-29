using Microsoft.EntityFrameworkCore;
using UserManagementService.Application.DTOs;
using UserManagementService.Application.Interfaces;
using UserManagementService.Domain;
using UserManagementService.Domain.Models;
using UserManagementService.Infrastructure.Repositories;

namespace UserManagementService.Infrastructure;

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
            //TODO convert.
            return user;
        }
        return null;
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
    {
        var response = await _dbContext.Users.FindAsync(userId);
        //TODO Implement convertion from user to userDTO
        //TODO Mapper or something else.
        return null;
    }

    public async Task<bool> CreateUserAsync(UserDTO user)
    {
        // Check if a user with the same email already exists
        if (await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
        {
            return false;
        }

        // Hash the password before saving
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
