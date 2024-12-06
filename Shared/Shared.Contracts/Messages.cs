using NServiceBus;

namespace Shared.Contracts;

public class UserCreatedEvent: IEvent
{
    public Guid UserId { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}


public class InitiatePaymentCommand
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
}