
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using UserManagementService.Infrastructure.Repositories;
using UserManagementService.Domain.Models;
using NServiceBus;


namespace UserManagementService.Web.Messaging
{
    public class UserCreatedEventHandler : IHandleMessages<UserCreatedEvent>
    {
        private readonly UserRepository _userRepository;

        public UserCreatedEventHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UserCreatedEvent message, IMessageHandlerContext context)
        {
            if (message?.Email is null || message.Email is null)
            {
                throw new ArgumentNullException(nameof(message));

            }
            Console.WriteLine($"UserCreatedEvent received for UserId: {message.UserId}");
            /* if (await _userRepository.GetUserByEmailAsync(message.Email) is not null)
             {
                 throw new ArgumentNullException(nameof(message));

             };*/

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = message.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(message.Password),
                Role = message.Role
            };

            Console.WriteLine($"UserCreatedEvent received for UserId: {newUser}");

            //await _userRepository.AddUserAsync(newUser);
            await Task.CompletedTask;
        }
    }
}