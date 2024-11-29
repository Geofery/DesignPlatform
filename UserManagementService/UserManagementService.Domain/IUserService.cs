namespace UserManagementService.Domain
{
    public interface IUserService
    {
        Task<User?> ValidateUserAsync(string email, string password);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<bool> CreateUserAsync(User user);
    }
}

