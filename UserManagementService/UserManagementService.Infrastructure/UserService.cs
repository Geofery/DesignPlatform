
using UserManagementService.Domain;
using System.Collections.Concurrent;

namespace UserManagementService.Infrastructure;

public class UserService : IUserService
{
    // Simulated in-memory storage for users
    private static readonly ConcurrentDictionary<string, User> _users = new();

    public UserService()
    {
        // Add a sample user for testing
        _users.TryAdd("test@example.com", new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), // Hashed password
            Role = "Customer"
        });
    }

    public async Task<User?> ValidateUserAsync(string email, string password)
    {
        if (_users.TryGetValue(email, out var user))
        {
            // Check password hash
            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return await Task.FromResult(user);
            }
        }
        return null;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var user = _users.Values.FirstOrDefault(u => u.Id == userId);
        return await Task.FromResult(user);
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        if (_users.ContainsKey(user.Email))
        {
            return await Task.FromResult(false); // User already exists
        }

        // Hash password before saving
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        _users.TryAdd(user.Email, user);

        return await Task.FromResult(true);
    }
}

