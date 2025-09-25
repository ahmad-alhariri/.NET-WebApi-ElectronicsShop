using ElectronicsShop.Domain.Common;

namespace ElectronicsShop.Domain.Users.Events;

public class UserCreatedEvent : BaseEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public UserCreatedEvent(Guid userId, string email, string firstName, string lastName)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}