using Microsoft.EntityFrameworkCore;
using UserManagementService.Domain;

    namespace UserManagementService.Infrastructure.Repositories;

    public class UserRepository : IUserRepository
    {
        private readonly UserManagementDbContext _dbContext;

        public UserRepository(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }

