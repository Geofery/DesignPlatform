using NServiceBus;
using Shared.Contracts;

namespace UserManagementService.Infrastructure.Handlers;

public class UserCreatedEventHandler : IHandleMessages<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent message, IMessageHandlerContext context)
    {
        // Process the event (e.g., log, update database, etc.)
        Console.WriteLine($"User Created: {message.UserId}, Email: {message.Email}, Role: {message.Role}");

        return Task.CompletedTask;
    }
}
