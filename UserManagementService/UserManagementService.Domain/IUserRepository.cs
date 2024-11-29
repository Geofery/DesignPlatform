using System;
using UserManagementService.Domain.Models;

namespace UserManagementService.Domain;

    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<bool> IsEmailExistsAsync(string email);
        Task AddUserAsync(User user);
    }

